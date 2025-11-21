using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using AutoNai3Tools.body;
using AutoNai3Tools.utils;

namespace AutoNai3Tools.Controllers {
    internal sealed class GenerationController {
        private readonly IGenerationDataProvider dataProvider;
        private GenerationPipeline currentPipeline;
        private CancellationTokenSource cancellationTokenSource;
        private readonly GenerationRequestPipeline requestPipeline;

        public GenerationController(IGenerationDataProvider dataProvider) {
            this.dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            requestPipeline = CreateRequestPipeline();
        }

        public bool IsGenerating => currentPipeline != null;

        public event Action Started;
        public event Action Stopped;
        public event Action<int> IterationStarted;
        public event Action<int, Bitmap> ImageReady;
        public event Action<int, DelayInfo, string> DelayPlanned;
        public event Action Completed;
        public event Action Cancelled;
        public event Action<Exception> Failed;

        public void StartGeneration() {
            if (IsGenerating)
                throw new InvalidOperationException("Generation is already running");

            GenerationInput input = dataProvider.CaptureInput();
            var context = new GenerationContext(
                dataProvider.PicProps,
                dataProvider.SettingProps,
                input.PromptText,
                input.NegativePromptText,
                input.Vibes,
                input.Img2Img,
                dataProvider.PicProps.RunNum);

            currentPipeline = new GenerationPipeline(context,
                index => requestPipeline.BuildRequest(context, dataProvider.PromptContext, index));
            AttachPipelineEvents();

            cancellationTokenSource = new CancellationTokenSource();
            Started?.Invoke();
            _ = currentPipeline.RunAsync(cancellationTokenSource.Token);
        }

        public void RequestStopGeneration() {
            if (!IsGenerating)
                return;

            cancellationTokenSource?.Cancel();
        }

        private void AttachPipelineEvents() {
            currentPipeline.IterationStarted += HandleIterationStarted;
            currentPipeline.ImageReady += HandleImageReady;
            currentPipeline.DelayPlanned += HandleDelayPlanned;
            currentPipeline.Completed += HandleCompleted;
            currentPipeline.Cancelled += HandleCancelled;
            currentPipeline.Failed += HandleFailed;
        }

        private void DetachPipelineEvents() {
            if (currentPipeline == null)
                return;

            currentPipeline.IterationStarted -= HandleIterationStarted;
            currentPipeline.ImageReady -= HandleImageReady;
            currentPipeline.DelayPlanned -= HandleDelayPlanned;
            currentPipeline.Completed -= HandleCompleted;
            currentPipeline.Cancelled -= HandleCancelled;
            currentPipeline.Failed -= HandleFailed;
        }

        private void HandleIterationStarted(int iteration) => IterationStarted?.Invoke(iteration);

        private void HandleImageReady(int iteration, Bitmap bitmap) => ImageReady?.Invoke(iteration, bitmap);

        private void HandleDelayPlanned(int iteration, DelayInfo delayInfo, string prompt) =>
            DelayPlanned?.Invoke(iteration, delayInfo, prompt);

        private void HandleCompleted() {
            Completed?.Invoke();
            StopInternal();
        }

        private void HandleCancelled() {
            Cancelled?.Invoke();
            StopInternal();
        }

        private void HandleFailed(Exception exception) {
            Failed?.Invoke(exception);
            StopInternal();
        }

        private void StopInternal() {
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            DetachPipelineEvents();
            currentPipeline = null;
            Stopped?.Invoke();
        }

        private GenerationRequestPipeline CreateRequestPipeline() {
            return new GenerationRequestPipeline()
                .AddStep(new SetRunNumberStep())
                .AddStep(new ResolutionStep())
                .AddStep(new NegativePromptStep())
                .AddStep(new Img2ImgStep())
                .AddStep(new VibeStep())
                .AddStep(new PromptStep())
                .AddStep(new BodyBuildStep());
        }
    }
}
