using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AutoNai3Tools.body;

namespace AutoNai3Tools.utils {
    internal class GenerationRequest {
        public GenerationRequest(BodyBase body, string originalPrompt, string noArtistPrompt, string artistSummary,
            GenerationRunInfo runInfo) {
            Body = body;
            OriginalPrompt = originalPrompt;
            NoArtistPrompt = noArtistPrompt;
            ArtistSummary = artistSummary;
            RunInfo = runInfo;
        }

        public BodyBase Body { get; }
        public string OriginalPrompt { get; }
        public string NoArtistPrompt { get; }
        public string ArtistSummary { get; }
        public GenerationRunInfo RunInfo { get; }
    }

    internal class GenerationRunInfo {
        public GenerationRunInfo(string model, int width, int height, float scale, float cfg, string sampler,
            int steps, string wildcardPath) {
            Model = model;
            Width = width;
            Height = height;
            Scale = scale;
            Cfg = cfg;
            Sampler = sampler;
            Steps = steps;
            WildcardPath = wildcardPath;
        }

        public string Model { get; }
        public int Width { get; }
        public int Height { get; }
        public float Scale { get; }
        public float Cfg { get; }
        public string Sampler { get; }
        public int Steps { get; }
        public string WildcardPath { get; }
    }

    internal class GenerationPipeline {
        private readonly GenerationContext _context;
        private readonly Func<int, GenerationRequest> _requestFactory;
        private readonly GenerationDelayStrategy _delayStrategy;
        private readonly NovalAi _novalAi = new NovalAi();

        public GenerationPipeline(GenerationContext context, Func<int, GenerationRequest> requestFactory) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _requestFactory = requestFactory ?? throw new ArgumentNullException(nameof(requestFactory));
            _delayStrategy = new GenerationDelayStrategy(context.SettingProps);
        }

        public event Action<int> IterationStarted;
        public event Action<int, Bitmap> ImageReady;
        public event Action<int, DelayInfo, string> DelayPlanned;
        public event Action Completed;
        public event Action Cancelled;
        public event Action<Exception> Failed;

        public async Task RunAsync(CancellationToken token) {
            try {
                for (int i = 0; i < _context.RunCount; i++) {
                    token.ThrowIfCancellationRequested();
                    int iterationNumber = i + 1;

                    GenerationRequest request;
                    try {
                        if (i > 0)
                            Logger.Spacer();

                        request = await Task.Run(() => _requestFactory(i), token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException) {
                        throw;
                    }
                    catch (Exception ex) {
                        Logger.Error("第" + iterationNumber + "次生成参数构建失败", exception: ex,
                            context: Logger.Context(("iteration", iterationNumber)));
                        continue;
                    }

                    IterationStarted?.Invoke(iterationNumber);

                    var runInfo = request.RunInfo;
                    Logger.Info(
                        $"本次参数 | 模型:{runInfo.Model} | 尺寸:{runInfo.Width}x{runInfo.Height} | Scale:{runInfo.Scale} | CFG:{runInfo.Cfg} | 采样:{runInfo.Sampler} | Steps:{runInfo.Steps} | Wildcard:{runInfo.WildcardPath}",
                        context: Logger.Context(("iteration", iterationNumber),
                            ("model", runInfo.Model),
                            ("width", runInfo.Width),
                            ("height", runInfo.Height),
                            ("scale", runInfo.Scale),
                            ("cfg", runInfo.Cfg),
                            ("sampler", runInfo.Sampler),
                            ("steps", runInfo.Steps),
                            ("wildcard", runInfo.WildcardPath)));

                    Logger.Info($"开始第{iterationNumber}次生成 | 原始Prompt：{request.OriginalPrompt}",
                        context: Logger.Context(("iteration", iterationNumber),
                            ("originalPrompt", request.OriginalPrompt)));
                    Bitmap bitmap = await _novalAi.SendGenerateRequestsAsync(
                            _context.SettingProps.Token,
                            request.Body,
                            request.NoArtistPrompt,
                            request.ArtistSummary,
                            _context.PicProps,
                            _context.SettingProps,
                            _context.SettingProps.Proxy,
                            request.OriginalPrompt)
                        .ConfigureAwait(false);

                    if (bitmap != null)
                        ImageReady?.Invoke(iterationNumber, bitmap);

                    token.ThrowIfCancellationRequested();

                    if (i == _context.RunCount - 1) {
                        Logger.Info($"生成任务已完成，共运行{iterationNumber}次",
                            context: Logger.Context(("totalIterations", iterationNumber)));
                        break;
                    }

                    DelayInfo delay = _delayStrategy.GetDelay(i);
                    DelayPlanned?.Invoke(iterationNumber, delay, request.OriginalPrompt);

                    string restType = delay.IsLongBreak ? "长休" : "短休";
                    Logger.Info($"已完成第{iterationNumber}次生成，进入{restType} {delay.Milliseconds} 毫秒",
                        context: Logger.Context(("iteration", iterationNumber),
                            ("originalPrompt", request.OriginalPrompt),
                            ("delayMs", delay.Milliseconds),
                            ("type", restType)));
                    await Task.Delay(delay.Milliseconds, token).ConfigureAwait(false);
                }

                Completed?.Invoke();
            }
            catch (OperationCanceledException) {
                Logger.Info("生成任务已取消，等待当前批次结束",
                    context: Logger.Context(("reason", "user")));
                Cancelled?.Invoke();
            }
            catch (Exception ex) {
                Logger.Error("生成任务异常", exception: ex);
                Failed?.Invoke(ex);
            }
        }
    }

    internal class GenerationDelayStrategy {
        private readonly SettingProperty _settings;
        private readonly Random _random = new Random();

        public GenerationDelayStrategy(SettingProperty settings) {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public DelayInfo GetDelay(int iterationIndex) {
            bool isLongBreak = iterationIndex != 0 && iterationIndex % 10 == 0;
            int lowSeconds = isLongBreak ? _settings.SleepTimeLongLow : _settings.SleepTimeShortLow;
            int highSeconds = isLongBreak ? _settings.SleepTimeLongHigh : _settings.SleepTimeShortHigh;
            if (highSeconds < lowSeconds) {
                Logger.Warn("检测到休眠时间设置错误，已自动校正",
                    context: Logger.Context(("iteration", iterationIndex),
                        ("low", lowSeconds), ("high", highSeconds)));
                highSeconds = lowSeconds;
            }

            int minSeconds = Math.Max(0, lowSeconds);
            int maxSeconds = Math.Max(minSeconds, highSeconds);
            int minMilliseconds = minSeconds * 1000;
            int maxMilliseconds = maxSeconds * 1000;
            if (maxMilliseconds <= minMilliseconds)
                maxMilliseconds = minMilliseconds + 1;

            int milliseconds = _random.Next(minMilliseconds, maxMilliseconds);
            return new DelayInfo(milliseconds, isLongBreak);
        }
    }

    internal class DelayInfo {
        public DelayInfo(int milliseconds, bool isLongBreak) {
            Milliseconds = milliseconds;
            IsLongBreak = isLongBreak;
        }

        public int Milliseconds { get; }
        public bool IsLongBreak { get; }
    }
}
