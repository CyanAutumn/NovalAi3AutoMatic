using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AutoNai3Tools.utils;

namespace AutoNai3Tools.Controllers {
    internal sealed class GenerationUiDataProvider : IGenerationDataProvider {
        private readonly PicProperty picProps;
        private readonly SettingProperty settingProps;
        private readonly IPromptContext promptContext;
        private readonly Func<string> promptAccessor;
        private readonly Func<string> negativePromptAccessor;
        private readonly DataGridView vibeGrid;
        private readonly Func<Img2ImgOptions> img2ImgFactory;

        public GenerationUiDataProvider(
            PicProperty picProps,
            SettingProperty settingProps,
            IPromptContext promptContext,
            Func<string> promptAccessor,
            Func<string> negativePromptAccessor,
            DataGridView vibeGrid,
            Func<Img2ImgOptions> img2ImgFactory) {
            this.picProps = picProps ?? throw new ArgumentNullException(nameof(picProps));
            this.settingProps = settingProps ?? throw new ArgumentNullException(nameof(settingProps));
            this.promptContext = promptContext ?? throw new ArgumentNullException(nameof(promptContext));
            this.promptAccessor = promptAccessor ?? throw new ArgumentNullException(nameof(promptAccessor));
            this.negativePromptAccessor = negativePromptAccessor ?? throw new ArgumentNullException(nameof(negativePromptAccessor));
            this.vibeGrid = vibeGrid;
            this.img2ImgFactory = img2ImgFactory;
        }

        public PicProperty PicProps => picProps;
        public SettingProperty SettingProps => settingProps;
        public IPromptContext PromptContext => promptContext;

        public GenerationInput CaptureInput() {
            var promptText = promptAccessor();
            var negativePrompt = negativePromptAccessor();
            var vibes = BuildVibeSelections();
            var img2Img = img2ImgFactory?.Invoke();
            return new GenerationInput(promptText, negativePrompt, vibes, img2Img);
        }

        private List<VibeSelection> BuildVibeSelections() {
            List<VibeSelection> selections = new List<VibeSelection>();
            if (vibeGrid == null)
                return selections;

            foreach (DataGridViewRow row in vibeGrid.Rows) {
                var picPath = row.Cells["Column1"].Value;
                if (picPath == null)
                    continue;

                float informationExtracted = ParseFloat(row.Cells["Column2"].Value);
                float referenceStrength = ParseFloat(row.Cells["Column3"].Value);
                selections.Add(new VibeSelection(picPath.ToString(), informationExtracted, referenceStrength));
            }

            return selections;
        }

        private static float ParseFloat(object value) {
            if (value == null)
                return 0f;

            if (value is float f)
                return f;
            if (value is double d)
                return (float)d;
            if (value is decimal dec)
                return (float)dec;

            return float.TryParse(value.ToString(), out var result) ? result : 0f;
        }
    }
}
