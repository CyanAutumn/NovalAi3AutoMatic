using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoNai3Tools.utils;
using Newtonsoft.Json;

namespace AutoNai3Tools.body {
    public class NovalAI : BodyBase {
        [JsonProperty("input")] public string prompt { get; set; }
        public string model { get; set; }
        public string action { get; set; }
        public GenerationParameters parameters { get; set; }

        public NovalAI(Dictionary<string, object> kwargs) : base(kwargs) {
            parameters = new GenerationParameters();
            SetProperties(this, kwargs);
            SetProperties(parameters, kwargs);
            parameters.params_version = 3;
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

                        if (!propertyType.IsPrimitive && propertyType != typeof(string)) {
                            value = Newtonsoft.Json.JsonConvert.DeserializeObject(value.ToString(), propertyType);
                        }
                        else if (value is IEnumerable<object> objList) {
                            // **如果是 List<object>，转换成目标 List<T>**
                            var convertedList = Activator.CreateInstance(propertyType) as System.Collections.IList;
                            foreach (var item in objList) {
                                convertedList.Add(Convert.ChangeType(item, propertyType));
                            }
                            value = convertedList;
                        }
                        else {
                            value = Convert.ChangeType(value, propertyType);
                        }

                        prop.SetValue(target, value);
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