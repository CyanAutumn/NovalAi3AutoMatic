using System;
using System.Collections.Generic;
using System.Linq;
using AutoNai3Tools.body;
using AutoNai3Tools.utils;

namespace AutoNai3Tools.Controllers {
    internal sealed class GenerationRequestBuilderContext {
        public GenerationRequestBuilderContext(GenerationContext sourceContext, IPromptContext promptContext, int runIndex) {
            SourceContext = sourceContext ?? throw new ArgumentNullException(nameof(sourceContext));
            PromptContext = promptContext ?? throw new ArgumentNullException(nameof(promptContext));
            RunIndex = runIndex;
            Kwargs = sourceContext.PicProps.GetProperty();
            OriginalPrompt = sourceContext.PromptText;
            NegativePrompt = sourceContext.NegativePrompt;
        }

        public GenerationContext SourceContext { get; }
        public IPromptContext PromptContext { get; }
        public int RunIndex { get; }
        public Dictionary<string, object> Kwargs { get; }
        public string OriginalPrompt { get; set; }
        public string NegativePrompt { get; set; }
        public string ResolvedPrompt { get; set; }
        public string NoArtistPrompt { get; set; }
        public BodyBase Body { get; set; }
        public GenerationRunInfo RunInfo { get; set; }
    }

    internal interface IGenerationRequestStep {
        void Apply(GenerationRequestBuilderContext context);
    }

    internal sealed class GenerationRequestPipeline {
        private readonly List<IGenerationRequestStep> steps = new List<IGenerationRequestStep>();

        public GenerationRequestPipeline AddStep(IGenerationRequestStep step) {
            steps.Add(step ?? throw new ArgumentNullException(nameof(step)));
            return this;
        }

        public GenerationRequest BuildRequest(GenerationContext context, IPromptContext promptContext, int runIndex) {
            var state = new GenerationRequestBuilderContext(context, promptContext, runIndex);
            foreach (var step in steps) {
                try {
                    step.Apply(state);
                }
                catch (Exception ex) {
                    Logger.Error("生成请求步骤执行失败", exception: ex,
                        context: Logger.Context(("step", step.GetType().Name), ("run", runIndex)));
                    throw;
                }
            }

            if (state.Body == null)
                throw new InvalidOperationException("未构建生成 Body。");
            if (state.RunInfo == null)
                throw new InvalidOperationException("未构建生成 RunInfo。");

            return new GenerationRequest(state.Body, state.OriginalPrompt, state.NoArtistPrompt, state.RunInfo);
        }
    }

    internal sealed class SetRunNumberStep : IGenerationRequestStep {
        public void Apply(GenerationRequestBuilderContext context) {
            context.PromptContext.SetRunNumber(context.RunIndex);
        }
    }

    internal sealed class ResolutionStep : IGenerationRequestStep {
        private readonly Random random = new Random();
        private int resolutionSelectIndex;

        public void Apply(GenerationRequestBuilderContext context) {
            var picProps = context.SourceContext.PicProps;
            var settingProps = context.SourceContext.SettingProps;
            int runNum = context.RunIndex;

            var originalWidth = picProps.Width;
            var originalHeight = picProps.Height;

            if (runNum == 0 || (runNum % picProps.RunKeepParams == 0 && settingProps.KeepResolution) ||
                !settingProps.KeepResolution) {
                var resolutionList = picProps.GetResolutionOptions().ToArray();
                if (resolutionList.Length == 0) {
                    Logger.Warn("分辨率列表为空，继续使用当前分辨率",
                        context: Logger.Context(("run", runNum)));
                    return;
                }

                if (picProps.ResolutionMode != ResolutionMode.固定) {
                    switch (picProps.ResolutionMode) {
                        case ResolutionMode.随机:
                            resolutionSelectIndex = random.Next(0, resolutionList.Length);
                            break;
                        case ResolutionMode.顺序:
                            resolutionSelectIndex = (resolutionSelectIndex + 1) % resolutionList.Length;
                            break;
                    }

                    string[] resolution = resolutionList[resolutionSelectIndex].Split('x');
                    if (resolution.Length >= 2 && int.TryParse(resolution[0], out int w) &&
                        int.TryParse(resolution[1], out int h)) {
                        picProps.Width = w;
                        picProps.Height = h;
                    }
                    else {
                        Logger.Warn("分辨率格式无效，已回退到上一次分辨率",
                            context: Logger.Context(("run", runNum),
                                ("raw", resolutionList[resolutionSelectIndex])));
                        picProps.Width = originalWidth;
                        picProps.Height = originalHeight;
                    }
                }
            }

            context.Kwargs["width"] = picProps.Width;
            context.Kwargs["height"] = picProps.Height;
        }
    }

    internal sealed class NegativePromptStep : IGenerationRequestStep {
        public void Apply(GenerationRequestBuilderContext context) {
            context.Kwargs["negative_prompt"] = context.NegativePrompt;
        }
    }

    internal sealed class Img2ImgStep : IGenerationRequestStep {
        public void Apply(GenerationRequestBuilderContext context) {
            if (!context.SourceContext.HasImg2Img)
                return;

            var img2Img = context.SourceContext.Img2Img;
            try {
                var base64 = Tools.ConvertImageToBase64(img2Img.ImagePath);
                if (string.IsNullOrWhiteSpace(base64)) {
                    Logger.Warn("Img2Img 图像转换失败，已跳过 img2img 字段",
                        context: Logger.Context(("path", img2Img.ImagePath), ("run", context.RunIndex)));
                    return;
                }

                context.Kwargs["image"] = base64;
                context.Kwargs["strength"] = img2Img.Strength;
                context.Kwargs["noise"] = img2Img.Noise;
            }
            catch (Exception ex) {
                Logger.Error("Img2Img 处理失败，已跳过 img2img 字段", exception: ex,
                    context: Logger.Context(("path", img2Img.ImagePath), ("run", context.RunIndex)));
            }
        }
    }

    internal sealed class VibeStep : IGenerationRequestStep {
        public void Apply(GenerationRequestBuilderContext context) {
            List<VibeData> vibes = context.SourceContext.Vibes.ConvertAll(v => new VibeData {
                imagePath = v.ImagePath,
                informationExtracted = v.InformationExtracted,
                referenceStrength = v.ReferenceStrength
            });

            if (vibes.Count == 0)
                return;

            try {
                vibes = Vibe.GetVibe(context.SourceContext.PicProps.Model, vibes, context.SourceContext.SettingProps.Token);
            }
            catch (Exception ex) {
                Logger.Error("Vibe 处理失败，已跳过参考图像", exception: ex,
                    context: Logger.Context(("run", context.RunIndex)));
                return;
            }

            List<string> referenceImages = new List<string>();
            List<float> referenceInformationExtracted = new List<float>();
            List<float> referenceStrengths = new List<float>();
            foreach (var vibe in vibes) {
                if (string.IsNullOrWhiteSpace(vibe.base64Image)) {
                    Logger.Warn("Vibe 图片转码为空，已跳过该项",
                        context: Logger.Context(("path", vibe.imagePath),
                            ("run", context.RunIndex)));
                    continue;
                }

                referenceImages.Add(vibe.base64Image);
                referenceInformationExtracted.Add(vibe.informationExtracted);
                referenceStrengths.Add(vibe.referenceStrength);
            }

            if (referenceImages.Count > 0) {
                context.Kwargs["reference_image_multiple"] = referenceImages;
                context.Kwargs["reference_information_extracted_multiple"] = referenceInformationExtracted;
                context.Kwargs["reference_strength_multiple"] = referenceStrengths;
            }
        }
    }

    internal sealed class PromptStep : IGenerationRequestStep {
        public void Apply(GenerationRequestBuilderContext context) {
            var prompt = Prompt.GetPrompt(context.OriginalPrompt, context.PromptContext);
            context.NoArtistPrompt = Prompt.GetNoArtistPrompt(prompt);
            context.ResolvedPrompt = Prompt.GetDataPrompt(prompt);

            context.Kwargs["prompt"] = context.ResolvedPrompt;
            context.Kwargs["v4_negative_prompt"] =
                new V4Prompt(new Caption(context.NegativePrompt, new List<CharCaption>()), null, null, false);
            context.Kwargs["v4_prompt"] = new V4Prompt(new Caption(context.ResolvedPrompt, new List<CharCaption>()), true, true, null);
        }
    }

    internal sealed class BodyBuildStep : IGenerationRequestStep {
        public void Apply(GenerationRequestBuilderContext context) {
            var picProps = context.SourceContext.PicProps;
            context.Body = BodyTools.GetBody(picProps.Model, context.Kwargs);

            context.RunInfo = new GenerationRunInfo(
                BodyTools.GetEnumDescription(picProps.Model),
                picProps.Width,
                picProps.Height,
                picProps.Scale,
                picProps.CFG,
                BodyTools.GetEnumDescription(picProps.Sampler),
                picProps.Steps,
                picProps.WildcardFolderPath);
        }
    }
}
