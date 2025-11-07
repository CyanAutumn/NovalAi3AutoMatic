using AutoNai3Tools.body;
using AutoNai3Tools.novalai;
using AutoNai3Tools.utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoNai3Tools.utils {
    internal class Vibe {
        private static string GetEncodingAliasFromModelName(string modelName) {
            switch (modelName) {
                case "nai-diffusion-4-curated-preview":
                    return "v4curated";
                case "nai-diffusion-4-full":
                    return "v4full";
                case "nai-diffusion-4-5-curated":
                    return "v4-5curated";
                case "nai-diffusion-4-5-full":
                    return "v4-5full";
                default:
                    return null;
            }
        }

        private static JObject GetEncodingBlock(JObject encodingsObj, string alias) {
            if (encodingsObj == null || string.IsNullOrEmpty(alias))
                return null;

            JToken modelToken = encodingsObj[alias];
            if (modelToken == null) {
                modelToken = encodingsObj.Properties()
                    .FirstOrDefault(p => string.Equals(p.Name, alias, StringComparison.OrdinalIgnoreCase))?.Value;
            }

            return modelToken as JObject;
        }

        private static JObject GetModelEncodings(JObject jsonObj, BodyTools.Model model) {
            if (jsonObj == null)
                return null;

            if (!(jsonObj["encodings"] is JObject encodingsObj))
                return null;

            string alias = null;
            try {
                alias = BodyTools.GetAnotherName(model);
            }
            catch {
                alias = null;
            }

            JObject modelEncodingObject = GetEncodingBlock(encodingsObj, alias);

            if (modelEncodingObject == null) {
                string importModelName = jsonObj["importInfo"]?["model"]?.ToString();
                string importAlias = GetEncodingAliasFromModelName(importModelName);
                modelEncodingObject = GetEncodingBlock(encodingsObj, importAlias);
            }

            if (modelEncodingObject == null) {
                modelEncodingObject = encodingsObj.Properties().FirstOrDefault()?.Value as JObject;
            }

            return modelEncodingObject;
        }

        private static string ResolveEncoding(JObject jsonObj, BodyTools.Model model, float informationExtracted) {
            var modelEncodingObject = GetModelEncodings(jsonObj, model);
            if (modelEncodingObject == null)
                return null;

            var properties = modelEncodingObject.Properties().ToList();
            JProperty fallbackProperty = properties.FirstOrDefault();
            foreach (var prop in properties) {
                var infoToken = prop.Value["params"]?["information_extracted"];
                if (infoToken != null && infoToken.Type != JTokenType.Null) {
                    float infoValue = infoToken.Value<float>();
                    if (Math.Abs(infoValue - informationExtracted) < 0.0001f) {
                        return prop.Value["encoding"]?.ToString();
                    }
                }
            }

            if (fallbackProperty == null)
                return null;

            var importInfoValue = jsonObj["importInfo"]?["information_extracted"]?.Value<float?>();
            if (!importInfoValue.HasValue || Math.Abs(importInfoValue.Value - informationExtracted) < 0.0001f || properties.Count == 1) {
                return fallbackProperty.Value["encoding"]?.ToString();
            }

            return null;
        }

        private static List<float> GetInformationExtractedOptions(JObject jsonObj, BodyTools.Model model) {
            var results = new List<float>();
            var modelEncodingObject = GetModelEncodings(jsonObj, model);
            if (modelEncodingObject != null) {
                foreach (var prop in modelEncodingObject.Properties()) {
                    var infoToken = prop.Value["params"]?["information_extracted"];
                    if (infoToken != null && infoToken.Type != JTokenType.Null) {
                        float value = infoToken.Value<float>();
                        if (!results.Contains(value))
                            results.Add(value);
                    }
                }
            }

            if (results.Count == 0) {
                var importInfoValue = jsonObj["importInfo"]?["information_extracted"]?.Value<float?>();
                if (importInfoValue.HasValue)
                    results.Add(importInfoValue.Value);
            }

            return results;
        }

        public static List<VibeData> ParseNai4_UP_Vibe(BodyTools.Model model, List<VibeData> vibe_list, Form1 form) {
            for (int i = 0; i < vibe_list.Count; i++) {
                if (vibe_list[i].imagePath.EndsWith(".naiv4vibe")) {
                    try {
                        string jsonContent = File.ReadAllText(vibe_list[i].imagePath);
                        JObject jsonObj = JObject.Parse(jsonContent);
                        var encoding = ResolveEncoding(jsonObj, model, vibe_list[i].informationExtracted);
                        if (!string.IsNullOrEmpty(encoding)) {
                            vibe_list[i].base64Image = encoding;
                        }
                        else {
                            Logger.Warn($"未能在 {vibe_list[i].imagePath} 中找到与信息抽取 {vibe_list[i].informationExtracted} 匹配的vibe编码");
                        }
                    }
                    catch (Exception ex) {
                        Logger.Error($"解析 {vibe_list[i].imagePath} 失败: {ex.Message}");
                    }
                }
                else {
                    var model_name = BodyTools.GetEnumDescription(model); ;
                    var vibe_name = Path.GetFileNameWithoutExtension(vibe_list[i].imagePath);
                    vibe_name = $"{vibe_name}_{model_name}_{vibe_list[i].informationExtracted}";
                    vibe_name = vibe_name.Replace(',', '_') + ".txt";
                    var vibe_path = Path.GetDirectoryName(vibe_list[i].imagePath) + $"\\{vibe_name}";

                    if (!File.Exists(vibe_path)) {
                        Logger.Info("未找到nai4+ vibe缓存文件，使用点数进行创建");

                        var base64img = Tools.ConvertImageToBase64(vibe_list[i].imagePath);
                        if (string.IsNullOrEmpty(base64img)) {
                            Logger.Error($"图片转换失败，跳过当前图片，路径为 {vibe_list[i]}");
                            continue;
                        }

                        var vibeBase64img = NovalAIAPI.GetVibeID(base64img, vibe_list[i].informationExtracted, model_name, form.txtToken.Text);
                        File.WriteAllText(vibe_path, vibeBase64img, Encoding.UTF8);
                        Logger.Info($"创建成功");
                    }

                    vibe_list[i].base64Image = File.ReadAllText(vibe_path, Encoding.UTF8);
                }
            }
            return vibe_list;
        }

        public static List<VibeData> ParseOtherVibe(BodyTools.Model model, List<VibeData> vibe_list) {
            for (int i = 0; i < vibe_list.Count; i++) {
                var base64img = Tools.ConvertImageToBase64(vibe_list[i].imagePath);
                vibe_list[i].base64Image = base64img;
                if (string.IsNullOrEmpty(base64img)) {
                    Logger.Error($"图片转换失败，跳过当前图片，路径为 {vibe_list[i]}");
                    continue;
                }
            }
            return vibe_list;
        }

        public static List<VibeData> GetVibe(BodyTools.Model model, List<VibeData> vibe_list, Form1 form) {
            if (model == BodyTools.Model.Nai4_Full || model == BodyTools.Model.Nai4_5_Full || model == BodyTools.Model.Nai4_5_Curated || model == BodyTools.Model.Nai4_Preview) {
                return ParseNai4_UP_Vibe(model, vibe_list, form);
            }
            return ParseOtherVibe(model, vibe_list);
        }

        public static void SetVibeInterfaceStatus(string vibe_path, Form1 form) {
            if (vibe_path.EndsWith(".naiv4vibe")) {
                try {
                    string jsonContent = File.ReadAllText(vibe_path);
                    JObject jsonObj = JObject.Parse(jsonContent);
                    var options = GetInformationExtractedOptions(jsonObj, form.picProps.Model);
                    form.cmbVibeIE.Items.Clear();
                    if (options.Count > 0) {
                        form.nudVibeIE.Visible = false;
                        form.cmbVibeIE.Visible = true;
                        foreach (var value in options)
                            form.cmbVibeIE.Items.Add(value);

                        form.cmbVibeIE.SelectedIndex = 0;
                        form.nudVibeIE.Value = (decimal)options[0];
                    }
                    else {
                        form.nudVibeIE.Visible = true;
                        form.cmbVibeIE.Visible = false;
                    }
                }
                catch (Exception ex) {
                    Logger.Warn($"读取vibe文件失败: {ex.Message}");
                    form.nudVibeIE.Visible = true;
                    form.cmbVibeIE.Visible = false;
                }
            }
            else {
                form.nudVibeIE.Visible = true;
                form.cmbVibeIE.Visible = false;
            }
        }

        public static string SelectAndMappingPicToPictureBox(Form1 form) {
            string path = Tools.SelectVibeFile();
            if (path != null) {
                Tools.ShowImage(path, form.picVibeView);
                SetVibeInterfaceStatus(path, form);
                return path;
            }
            return null;
        }
    }
}
