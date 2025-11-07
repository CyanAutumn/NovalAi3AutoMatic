using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AutoNai3Tools.utils {
    internal class DirectorToolExecutionOptions {
        public int Iterations { get; set; }
        public string ColorizePrompt { get; set; }
        public int ColorizeDefry { get; set; }
        public string Emotion { get; set; }
        public string EmotionPrompt { get; set; }
        public int EmotionDefry { get; set; }
        public string Token { get; set; }
        public string Proxy { get; set; }
    }

    internal class DirectorToolProcessor {
        private readonly NovalAi novalAi = new NovalAi();

        public Task<Image> LoadPreviewAsync(string path, CancellationToken cancellationToken = default) {
            return Task.Run(() => LoadPreview(path, cancellationToken), cancellationToken);
        }

        private Image LoadPreview(string path, CancellationToken cancellationToken) {
            try {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (MemoryStream ms = new MemoryStream()) {
                    cancellationToken.ThrowIfCancellationRequested();
                    fs.CopyTo(ms);
                    cancellationToken.ThrowIfCancellationRequested();
                    ms.Position = 0;
                    using (var img = Image.FromStream(ms)) {
                        return new Bitmap(img);
                    }
                }
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                Logger.Warn("无法加载导演工具输入预览",
                    context: Logger.Context(("path", path), ("reason", ex.Message)));
                return null;
            }
        }

        public async Task<Bitmap> ExecuteAsync(string imagePath, int type, DirectorToolExecutionOptions options, PicProperty picProps,
            CancellationToken cancellationToken = default) {
            try {
                cancellationToken.ThrowIfCancellationRequested();
                string base64img = await Task.Run(() => Tools.ConvertImageToBase64(imagePath), cancellationToken)
                    .ConfigureAwait(false);
                if (string.IsNullOrEmpty(base64img))
                    return null;

                int width;
                int height;

                using (Image image = Image.FromFile(imagePath)) {
                    width = image.Width;
                    height = image.Height;
                }

                Nai3DirectorToolsBody body = BuildDirectorToolsBody(type, height, width, base64img, options);
                if (body == null)
                    return null;

                cancellationToken.ThrowIfCancellationRequested();
                return await novalAi.SendDirectorToolsRequestsAsync(options.Token, body, picProps, options.Proxy,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                Logger.Error("导演工具执行失败", exception: ex,
                    context: Logger.Context(("path", imagePath), ("type", type)));
                return null;
            }
        }

        private Nai3DirectorToolsBody BuildDirectorToolsBody(int type, int height, int width, string base64img,
            DirectorToolExecutionOptions options) {
            if (type == 0 || type == 1 || type == 2 || type == 5)
                return new Nai3DirectorToolsBody(height, width, base64img, GetBodyType(type));
            if (type == 3)
                return new Nai3DirectorToolsBody(height, width, base64img, GetBodyType(type),
                    options.ColorizePrompt, options.ColorizeDefry);
            if (type == 4)
                return new Nai3DirectorToolsBody(height, width, base64img, GetBodyType(type),
                    $"{options.Emotion};;{options.EmotionPrompt}", options.EmotionDefry);

            return null;
        }

        private string GetBodyType(int input) {
            switch (input) {
                case 0:
                    return "bg-removal";
                case 1:
                    return "lineart";
                case 2:
                    return "sketch";
                case 3:
                    return "colorize";
                case 4:
                    return "emotion";
                case 5:
                    return "declutter";
            }

            return null;
        }
    }
}
