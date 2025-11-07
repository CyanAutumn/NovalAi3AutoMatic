using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoNai3Tools.utils;

namespace AutoNai3Tools.Controllers {
    internal sealed class DirectorToolController {
        private readonly DirectorToolProcessor processor;
        private readonly PicProperty picProperty;

        public DirectorToolController(DirectorToolProcessor processor, PicProperty picProperty) {
            this.processor = processor ?? throw new ArgumentNullException(nameof(processor));
            this.picProperty = picProperty ?? throw new ArgumentNullException(nameof(picProperty));
        }

        public event Action<bool> BusyStateChanged;
        public event Action<Image> PreviewUpdated;
        public event Action<Bitmap> OutputUpdated;
        public event Action Completed;
        public event Action<Exception> Failed;

        public Task RunSingleAsync(string imagePath, int type, DirectorToolExecutionOptions options,
            CancellationToken cancellationToken = default) {
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new ArgumentException("imagePath 不能为空", nameof(imagePath));

            return RunInternalAsync(new[] { imagePath }, type, options, cancellationToken);
        }

        public Task RunBatchAsync(IEnumerable<string> imagePaths, int type, DirectorToolExecutionOptions options,
            CancellationToken cancellationToken = default) {
            if (imagePaths == null)
                throw new ArgumentNullException(nameof(imagePaths));

            var pathList = imagePaths.Where(path => !string.IsNullOrWhiteSpace(path)).Distinct().ToList();
            if (pathList.Count == 0)
                throw new ArgumentException("未提供有效的图片路径", nameof(imagePaths));

            return RunInternalAsync(pathList, type, options, cancellationToken);
        }

        private async Task RunInternalAsync(IReadOnlyList<string> imagePaths, int type,
            DirectorToolExecutionOptions options, CancellationToken cancellationToken) {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (options.Iterations <= 0)
                throw new InvalidOperationException("迭代次数必须大于 0");

            BusyStateChanged?.Invoke(true);
            try {
                foreach (var imagePath in imagePaths) {
                    cancellationToken.ThrowIfCancellationRequested();

                    var preview = await processor.LoadPreviewAsync(imagePath, cancellationToken).ConfigureAwait(false);
                    if (preview != null)
                        PreviewUpdated?.Invoke(preview);

                    await ExecuteImageAsync(imagePath, type, options, cancellationToken).ConfigureAwait(false);
                }

                Completed?.Invoke();
            }
            catch (OperationCanceledException ex) {
                Logger.Info("导演工具任务已取消",
                    context: Logger.Context(("reason", "user"), ("type", type)));
                Failed?.Invoke(ex);
            }
            catch (Exception ex) {
                Logger.Error("导演工具执行失败", exception: ex,
                    context: Logger.Context(("type", type)));
                Failed?.Invoke(ex);
            }
            finally {
                BusyStateChanged?.Invoke(false);
            }
        }

        private async Task ExecuteImageAsync(string imagePath, int type, DirectorToolExecutionOptions options,
            CancellationToken cancellationToken) {
            for (int i = 0; i < options.Iterations; i++) {
                cancellationToken.ThrowIfCancellationRequested();
                Bitmap img = await processor.ExecuteAsync(imagePath, type, options, picProperty, cancellationToken)
                    .ConfigureAwait(false);
                if (img != null)
                    OutputUpdated?.Invoke(img);
            }
        }
    }
}
