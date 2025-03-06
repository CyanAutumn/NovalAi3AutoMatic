using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.body {
    public class NovalAI : BodyBase{
        public string input { get; set; }
        public string model { get; set; }
        public string action { get; set; }
        public GenerationParameters parameters { get; set; }

        public NovalAI(Dictionary<string, object> kwargs) : base(kwargs) {
            this.input = kwargs.TryGetValue("input",out this.input);

        }
    }

    public class GenerationParameters {
        public int params_version { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public double scale { get; set; }
        public string sampler { get; set; }
        public int steps { get; set; }
        public long seed { get; set; }
        public int n_samples { get; set; }

        public int uc_preset { get; set; }
        public bool quality_toggle { get; set; }
        public bool sm { get; set; }
        public bool sm_dyn { get; set; }
        public bool dynamic_thresholding { get; set; }
        public double controlnet_strength { get; set; }
        public bool legacy { get; set; }
        public bool add_original_image { get; set; }
        public double cfg_rescale { get; set; }
        public string noise_schedule { get; set; }
        public bool legacy_v3_extend { get; set; }
        public double? skip_cfg_above_sigma { get; set; }
        public List<string> character_prompts { get; set; }
        public string negative_prompt { get; set; }
        public List<string> reference_image_multiple { get; set; }
        public List<string> reference_information_extracted_multiple { get; set; }
        public List<double> reference_strength_multiple { get; set; }
        public bool deliberate_euler_ancestral_bug { get; set; }
        public bool prefer_brownian { get; set; }

        public GenerationParameters() {
            character_prompts = new List<string>();
            reference_image_multiple = new List<string>();
            reference_information_extracted_multiple = new List<string>();
            reference_strength_multiple = new List<double>();
        }
    }
}