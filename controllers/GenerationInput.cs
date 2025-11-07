using System.Collections.Generic;
using AutoNai3Tools.utils;

namespace AutoNai3Tools.Controllers {
    internal sealed class GenerationInput {
        public GenerationInput(string promptText, string negativePromptText, List<VibeSelection> vibes,
            Img2ImgOptions img2Img) {
            PromptText = promptText ?? string.Empty;
            NegativePromptText = negativePromptText ?? string.Empty;
            Vibes = vibes ?? new List<VibeSelection>();
            Img2Img = img2Img;
        }

        public string PromptText { get; }
        public string NegativePromptText { get; }
        public List<VibeSelection> Vibes { get; }
        public Img2ImgOptions Img2Img { get; }
    }
}
