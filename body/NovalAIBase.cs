using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoNai3Tools.utils;
using Newtonsoft.Json;

namespace AutoNai3Tools.body {
    public class NovalAIBase : BodyBase {
        [JsonProperty("input")] public string prompt { get; set; }
        public string model { get; set; }
        public string action { get; set; }
        public GenerationParameters parameters { get; set; }

        public NovalAIBase(Dictionary<string, object> kwargs) : base(kwargs) {
            parameters = new GenerationParameters();
            SetProperties(this, kwargs);
            SetProperties(parameters, kwargs);
            parameters.params_version = 3;
            parameters.add_original_image = true;
            parameters.controlnet_strength = 1;
            parameters.n_samples = 1;
            parameters.qualityToggle = true;
            parameters.ucPreset = 0;
            if (kwargs.ContainsKey("image")) {
                this.action = "img2img";
                parameters.prefer_brownian = false;
                Random random = new Random();
                parameters.extra_noise_seed = random.Next(0, 1000000000);
                ;
            }
            else {
                this.action = "generate";
            }
        }

        private void SetProperties(object target, Dictionary<string, object> values) {
            Type type = target.GetType();
            foreach (var key in values.Keys) {
                PropertyInfo prop = type.GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.CanWrite) {
                    try {
                        object value = values[key];
                        if (value == null) {
                            continue;
                        }

                        Type propertyType = prop.PropertyType;

                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>)) {
                            var elementType = propertyType.GetGenericArguments()[0];

                            if (value is System.Collections.IEnumerable enumerable && !(value is string)) {
                                var list = prop.GetValue(target) as System.Collections.IList;

                                foreach (var item in enumerable) {
                                    if (item == null) {
                                        list.Add(null);
                                    }
                                    else if (elementType.IsPrimitive || elementType == typeof(string)) {
                                        list.Add(Convert.ChangeType(item, elementType));
                                    }
                                    else {
                                        list.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(
                                            Newtonsoft.Json.JsonConvert.SerializeObject(item), elementType));
                                    }
                                }
                            }
                            else {
                                var list = prop.GetValue(target) as System.Collections.IList;
                                if (elementType.IsPrimitive || elementType == typeof(string)) {
                                    list.Add(Convert.ChangeType(value, elementType));
                                }
                                else {
                                    list.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(
                                        Newtonsoft.Json.JsonConvert.SerializeObject(value), elementType));
                                }
                            }
                        }
                        else if (!propertyType.IsPrimitive && propertyType != typeof(string)) {
                            if (propertyType.IsGenericType &&
                                propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                                var underlyingType = Nullable.GetUnderlyingType(propertyType);
                                if (underlyingType != null) {
                                    value = Convert.ChangeType(value, underlyingType);
                                }
                            }
                            else {
                                value = Newtonsoft.Json.JsonConvert.DeserializeObject(
                                    Newtonsoft.Json.JsonConvert.SerializeObject(value), propertyType);
                            }
                        }
                        else {
                            value = Convert.ChangeType(value, propertyType);
                        }

                        if (!propertyType.IsGenericType || propertyType.GetGenericTypeDefinition() != typeof(List<>)) {
                            prop.SetValue(target, value);
                        }
                    }
                    catch (Exception ex) {
                        Logger.Error($"属性 {key} 赋值失败: {ex.Message}");
                    }
                }
            }
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

        public int extra_noise_seed { get; set; }
        public int ucPreset { get; set; }
        public bool prefer_brownian { get; set; }
        public bool qualityToggle { get; set; }
        public bool? sm { get; set; }
        public bool? sm_dyn { get; set; }
        public bool dynamic_thresholding { get; set; }
        public double controlnet_strength { get; set; }
        public bool legacy { get; set; }
        public bool add_original_image { get; set; }
        public double cfg_rescale { get; set; }
        public string noise_schedule { get; set; }
        public string image { get; set; }
        public float? strength { get; set; }
        public float? noise { get; set; }
        public bool legacy_v3_extend { get; set; }
        public double? skip_cfg_above_sigma { get; set; }
        public bool? use_coords { get; set; }
        public List<string> characterPrompts { get; set; }
        public string negative_prompt { get; set; }
        public List<string> reference_image_multiple { get; set; }
        public List<float> reference_information_extracted_multiple { get; set; }
        public List<float> reference_strength_multiple { get; set; }
        public bool deliberate_euler_ancestral_bug { get; set; }
        public V4Prompt v4_negative_prompt { get; set; }
        public V4Prompt v4_prompt { get; set; }

        public GenerationParameters() {
            characterPrompts = new List<string>();
            reference_image_multiple = new List<string>();
            reference_information_extracted_multiple = new List<float>();
            reference_strength_multiple = new List<float>();
        }
    }

    public class Position {
        public float x { get; set; }
        public float y { get; set; }
    }

    public class CharCaption {
        public List<Position> centers { get; set; }
        public string char_caption { get; set; }
    }

    public class Caption {
        public string base_caption { get; set; }
        public List<CharCaption> char_captions { get; set; }

        public Caption(string base_caption, List<CharCaption> char_captions) {
            this.base_caption = base_caption;
            this.char_captions = char_captions;
        }
    }

    public class V4Prompt {
        public Caption caption { get; set; }
        public bool? use_coords { get; set; }
        public bool? use_order { get; set; }
        public bool? legacy_uc { get; set; }

        public V4Prompt(Caption cattion, bool? use_coords, bool? use_order, bool? legacy_uc) {
            this.caption = cattion;
            this.use_coords=use_coords;
            this.use_order=use_order;
            this.legacy_uc=legacy_uc;
        }
    }
}