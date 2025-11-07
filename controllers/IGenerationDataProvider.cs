using AutoNai3Tools.utils;

namespace AutoNai3Tools.Controllers {
    internal interface IGenerationDataProvider {
        PicProperty PicProps { get; }
        SettingProperty SettingProps { get; }
        IPromptContext PromptContext { get; }
        GenerationInput CaptureInput();
    }
}
