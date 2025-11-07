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
        public static List<VibeData> ParseNai4_UP_Vibe(BodyTools.Model model, List<VibeData> vibe_list, Form1 form) {
            for (int i = 0; i < vibe_list.Count; i++) {
                if (vibe_list[i].imagePath.EndsWith(".naiv4vibe")) {
                    string jsonContent = File.ReadAllText(vibe_list[i].imagePath);
                    JObject jsonObj = JObject.Parse(jsonContent);
                    JToken modelEncodingToken = jsonObj["encodings"]?[BodyTools.GetAnotherName(form.picProps.Model)];
                    if (modelEncodingToken is JObject modelEncodingObject) {
                        foreach (var prop in modelEncodingObject.Properties()) {
                            if((float)prop.Value["params"]["information_extracted"] == vibe_list[i].informationExtracted) {
                                vibe_list[i].base64Image = prop.Value["encoding"].ToString();
                                break;
                            }
                        }
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
                form.nudVibeIE.Visible = false;
                form.cmbVibeIE.Visible = true;
                string jsonContent = File.ReadAllText(vibe_path);
                JObject jsonObj = JObject.Parse(jsonContent);
                JToken modelEncodingToken = jsonObj["encodings"]?[BodyTools.GetAnotherName(form.picProps.Model)];
                form.cmbVibeIE.Items.Clear();
                if (modelEncodingToken is JObject modelEncodingObject) {
                    foreach (var prop in modelEncodingObject.Properties()) {
                        form.cmbVibeIE.Items.Add(prop.Value["params"]["information_extracted"]);
                    }
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
