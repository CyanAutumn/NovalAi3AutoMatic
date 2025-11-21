using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoNai3Tools.utils {
    internal class GenerationContext {
        public GenerationContext(
            PicProperty picProps,
            SettingProperty settingProps,
            string promptText,
            string negativePrompt,
            IEnumerable<VibeSelection> vibes,
            Img2ImgOptions img2Img,
            int runCount) {
            PicProps = picProps ?? throw new ArgumentNullException(nameof(picProps));
            SettingProps = settingProps ?? throw new ArgumentNullException(nameof(settingProps));
            RunCount = Math.Max(1, runCount);
            UpdateDynamicInput(promptText, negativePrompt, vibes, img2Img);
        }

        public PicProperty PicProps { get; }
        public SettingProperty SettingProps { get; }
        public string PromptText { get; private set; }
        public string NegativePrompt { get; private set; }
        public List<VibeSelection> Vibes { get; private set; }
        public Img2ImgOptions Img2Img { get; private set; }
        public int RunCount { get; }
        public bool HasImg2Img => Img2Img != null && !string.IsNullOrWhiteSpace(Img2Img.ImagePath);

        public void UpdateDynamicInput(string promptText, string negativePrompt, IEnumerable<VibeSelection> vibes,
            Img2ImgOptions img2Img) {
            PromptText = promptText ?? string.Empty;
            NegativePrompt = negativePrompt ?? string.Empty;
            Vibes = vibes?.Select(CloneVibe).ToList() ?? new List<VibeSelection>();
            Img2Img = img2Img == null
                ? null
                : new Img2ImgOptions(img2Img.ImagePath, img2Img.Strength, img2Img.Noise);
        }

        private static VibeSelection CloneVibe(VibeSelection source) {
            if (source == null)
                return null;
            return new VibeSelection(source.ImagePath, source.InformationExtracted, source.ReferenceStrength);
        }
    }

    internal class VibeSelection {
        public VibeSelection(string imagePath, float informationExtracted, float referenceStrength) {
            ImagePath = imagePath;
            InformationExtracted = informationExtracted;
            ReferenceStrength = referenceStrength;
        }

        public string ImagePath { get; }
        public float InformationExtracted { get; }
        public float ReferenceStrength { get; }
    }

    internal class Img2ImgOptions {
        public Img2ImgOptions(string imagePath, float strength, float noise) {
            ImagePath = imagePath;
            Strength = strength;
            Noise = noise;
        }

        public string ImagePath { get; }
        public float Strength { get; }
        public float Noise { get; }
    }
}
