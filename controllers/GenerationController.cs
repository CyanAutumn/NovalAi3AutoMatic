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
        private int resolutionSelectIndex;
        private readonly Random resolutionRandom = new Random();

        public GenerationController(IGenerationDataProvider dataProvider) {
            this.dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
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

            currentPipeline = new GenerationPipeline(context, index => BuildGenerationRequest(context, index));
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

        private GenerationRequest BuildGenerationRequest(GenerationContext context, int runIndex) {
            dataProvider.PromptContext.SetRunNumber(runIndex);
            _ = GetResolution(runIndex);
            Dictionary<string, object> kwargs = context.PicProps.GetProperty();
            kwargs["negative_prompt"] = context.NegativePrompt;

            if (context.HasImg2Img) {
                kwargs["image"] = Tools.ConvertImageToBase64(context.Img2Img.ImagePath);
                kwargs["strength"] = context.Img2Img.Strength;
                kwargs["noise"] = context.Img2Img.Noise;
            }

            List<VibeData> vibes = context.Vibes.ConvertAll(v => new VibeData {
                imagePath = v.ImagePath,
                informationExtracted = v.InformationExtracted,
                referenceStrength = v.ReferenceStrength
            });

            if (vibes.Count > 0)
                vibes = Vibe.GetVibe(context.PicProps.Model, vibes, context.SettingProps.Token);

            List<string> t_rim = new List<string>();
            List<float> t_riem = new List<float>();
            List<float> t_rsm = new List<float>();
            foreach (var vibe in vibes) {
                t_rim.Add(vibe.base64Image);
                t_riem.Add(vibe.informationExtracted);
                t_rsm.Add(vibe.referenceStrength);
            }
            if (t_rim.Count > 0) {
                kwargs["reference_image_multiple"] = t_rim;
                kwargs["reference_information_extracted_multiple"] = t_riem;
                kwargs["reference_strength_multiple"] = t_rsm;
            }

            var prompt = Prompt.GetPrompt(context.PromptText, dataProvider.PromptContext);
            string noArtistPrompt = Prompt.GetNoArtistPrompt(prompt);
            string tPrompt = Prompt.GetDataPrompt(prompt);
            kwargs["prompt"] = tPrompt;

            kwargs["v4_negative_prompt"] =
                new V4Prompt(new Caption(context.NegativePrompt, new List<CharCaption>()), null, null, false);
            kwargs["v4_prompt"] = new V4Prompt(new Caption(tPrompt, new List<CharCaption>()), true, true, null);
            BodyBase body = BodyTools.GetBody(context.PicProps.Model, kwargs);

            var runInfo = new GenerationRunInfo(
                BodyTools.GetEnumDescription(context.PicProps.Model),
                context.PicProps.Width,
                context.PicProps.Height,
                context.PicProps.Scale,
                context.PicProps.CFG,
                BodyTools.GetEnumDescription(context.PicProps.Sampler),
                context.PicProps.Steps,
                context.PicProps.WildcardFolderPath);

            return new GenerationRequest(body, context.PromptText, noArtistPrompt, runInfo);
        }

        private int[] GetResolution(int runNum) {
            var picProps = dataProvider.PicProps;
            var settingProps = dataProvider.SettingProps;
            if (runNum == 0 || (runNum % picProps.RunKeepParams == 0 && settingProps.KeepResolution) ||
                !settingProps.KeepResolution) {
                var resolutionList = picProps.ResolutionList.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                if (picProps.ResolutionMode != ResolutionMode.固定) {
                    switch (picProps.ResolutionMode) {
                        case ResolutionMode.随机:
                            resolutionSelectIndex = resolutionRandom.Next(0, resolutionList.Length);
                            break;
                        case ResolutionMode.顺序:
                            resolutionSelectIndex = (resolutionSelectIndex + 1) % resolutionList.Length;
                            break;
                    }

                    string[] _Resolution = resolutionList[resolutionSelectIndex].Split('x');
                    picProps.Width = int.Parse(_Resolution[0]);
                    picProps.Height = int.Parse(_Resolution[1]);
                }
            }

            return new int[] { picProps.Width, picProps.Height };
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
    }
}
