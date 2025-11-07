using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AutoNai3Tools.body;

namespace AutoNai3Tools.utils {
    internal class GenerationRequest {
        public GenerationRequest(BodyBase body, string prompt, string noArtistPrompt) {
            Body = body;
            Prompt = prompt;
            NoArtistPrompt = noArtistPrompt;
        }

        public BodyBase Body { get; }
        public string Prompt { get; }
        public string NoArtistPrompt { get; }
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
                await Task.Run(async () => {
                    for (int i = 0; i < _context.RunCount; i++) {
                        token.ThrowIfCancellationRequested();
                        int iterationNumber = i + 1;

                        GenerationRequest request;
                        try {
                            request = _requestFactory(i);
                        }
                        catch (Exception ex) {
                            Logger.Error("参数错误：" + ex);
                            Logger.Info(
                                "-----------------------------------------------------------------------------------------------------------------------------------------");
                            continue;
                        }

                        IterationStarted?.Invoke(iterationNumber);

                        Logger.Info("开始发送生图请求");
                        Bitmap bitmap = _novalAi.SendGenerateRequests(
                            _context.SettingProps.Token,
                            request.Body,
                            request.NoArtistPrompt,
                            _context.PicProps,
                            _context.SettingProps.Proxy);

                        if (bitmap != null)
                            ImageReady?.Invoke(iterationNumber, bitmap);

                        token.ThrowIfCancellationRequested();

                        if (i == _context.RunCount - 1) {
                            Logger.Info($"运行完毕，共运行{iterationNumber}次");
                            break;
                        }

                        DelayInfo delay = _delayStrategy.GetDelay(i);
                        string prompt = request.Body?.prompt ?? string.Empty;
                        DelayPlanned?.Invoke(iterationNumber, delay, prompt);

                        string restType = delay.IsLongBreak ? "长休" : "短休";
                        Logger.Info(
                            $"图片信息：{prompt}\r\n已运行{iterationNumber}次，开始{restType}{delay.Milliseconds}毫秒");
                        await Task.Delay(delay.Milliseconds, token);
                        Logger.Info(
                            "-----------------------------------------------------------------------------------------------------------------------------------------");
                    }
                }, token);

                Completed?.Invoke();
            }
            catch (OperationCanceledException) {
                Logger.Info("已停止，等待当前生成结束");
                Cancelled?.Invoke();
            }
            catch (Exception ex) {
                Logger.Error($"生成任务异常：{ex}");
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
                Logger.Info("设置页面中的休息时间左侧不得大于右侧，已自动更改完毕");
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
