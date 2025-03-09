using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using AutoNai3Tools.body;

namespace AutoNai3Tools.utils {
    class Nai3Body {
        public string ToJson() {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    class Nai3GenerateImageBody : Nai3Body {
        public string input { get; set; }
        public string model { get; set; }
        public string action { get; set; }
        public Nai3Parmeters parameters = null;

        public Nai3GenerateImageBody(string input = "1girl", string model = "nai-diffusion-3",
            string action = "generate", Nai3Parmeters parameters = null) {
            this.input = input;
            this.model = model;
            this.action = action;
            this.parameters = parameters;
        }
    }

    class Nai3Parmeters {
        public int params_version { get; set; } = 3;
        public int width { get; set; } = 832;
        public int height { get; set; } = 1216;
        public List<string> characterPrompts { get; set; }
        public float scale { get; set; } = 5;
        public string sampler { get; set; } = "k_euler";
        public int steps { get; set; } = 28;
        public int extra_noise_seed { get; set; } = 0;
        public int n_samples { get; set; } = 1;
        public int ucPreset { get; set; } = 0;
        public bool qualityToggle { get; set; } = true;
        public bool sm { get; set; } = false;
        public bool sm_dyn { get; set; } = false;
        public bool dynamic_thresholding { get; set; } = false;
        public int controlnet_strength { get; set; } = 1;
        public bool legacy { get; set; } = false;
        public bool add_original_image { get; set; } = true;
        public int uncond_scale { get; set; } = 1;
        public float cfg_rescale { get; set; } = 0;
        public string noise_schedule { get; set; } = "karras";
        public bool legacy_v3_extend { get; set; } = false;
        public string image { get; set; } = null;
        public float? strength { get; set; } = null;
        public float? noise { get; set; } = null;
        public int seed { get; set; } = 0;
        public int? skip_cfg_above_sigma { get; set; }
        public bool deliberate_euler_ancestral_bug { get; set; } = false;
        public bool prefer_brownian { get; set; } = true;
        public string negative_prompt { get; set; } = null;
        public V4Prompt v4_negative_prompt { get; set; } = null;
        public V4Prompt v4_prompt { get; set; } = null;
        public List<string> reference_image_multiple { get; set; } = null;
        public List<float> reference_information_extracted_multiple { get; set; } = null;
        public List<float> reference_strength_multiple { get; set; } = null;

        public Nai3Parmeters(
            int params_version = 1,
            int width = 832,
            int height = 1216,
            float scale = 5,
            string sampler = "k_euler",
            int steps = 28,
            int n_samples = 1,
            int ucPreset = 0,
            bool qualityToggle = true,
            bool sm = false,
            bool sm_dyn = false,
            bool dynamic_thresholding = false,
            int controlnet_strength = 1,
            bool legacy = false,
            bool add_original_image = true,
            int uncond_scale = 1,
            float cfg_rescale = 0,
            string noise_schedule = "karras",
            bool legacy_v3_extend = false,
            string image = null,
            float? strength = null,
            float? noise = null,
            int? seed = null,
            int? skip_cfg_above_sigma = null,
            string negative_prompt = null,
            List<string> reference_image_multiple = null,
            List<float> reference_information_extracted_multiple = null,
            List<float> reference_strength_multiple = null,
            List<string> characterPrompts = null,
            V4Prompt v4_negative_prompt = null,
            V4Prompt v4_prompt = null
        ) {
            this.width = width;
            this.height = height;
            this.scale = scale;
            this.sampler = sampler;
            this.steps = steps;
            this.n_samples = n_samples;
            this.ucPreset = ucPreset;
            this.qualityToggle = qualityToggle;
            this.sm = sm;
            this.sm_dyn = sm_dyn;
            this.dynamic_thresholding = dynamic_thresholding;
            this.controlnet_strength = controlnet_strength;
            this.legacy = legacy;
            this.add_original_image = add_original_image;
            this.uncond_scale = uncond_scale;
            this.cfg_rescale = cfg_rescale;
            this.noise_schedule = noise_schedule;
            this.legacy_v3_extend = legacy_v3_extend;
            this.image = image;
            this.strength = strength;
            this.noise = noise;
            Random random = new Random();
            if (seed == null) {
                this.seed = random.Next(0, 1000000000);
            }
            else {
                this.seed = (int)seed;
            }

            this.skip_cfg_above_sigma = skip_cfg_above_sigma;
            this.extra_noise_seed = this.seed;
            this.negative_prompt = negative_prompt;
            this.reference_image_multiple = new List<string>();
            this.characterPrompts = new List<string>();
            if (reference_image_multiple != null)
                this.reference_image_multiple.AddRange(reference_image_multiple);
            this.reference_information_extracted_multiple = new List<float>();
            if (reference_information_extracted_multiple != null)
                this.reference_information_extracted_multiple.AddRange(reference_information_extracted_multiple);
            this.reference_strength_multiple = new List<float>();
            if (reference_strength_multiple != null)
                this.reference_information_extracted_multiple.AddRange(reference_strength_multiple);
            if (characterPrompts != null)
                this.characterPrompts.AddRange(characterPrompts);
        }
    }

    class Caption {
        public string base_caption { get; set; }
        public List<string> char_captions { get; set; }

        public Caption(string base_caption, List<string> char_captions = null) {
            this.base_caption = base_caption;
            if (char_captions == null)
                this.char_captions = new List<string>();
        }
    }

    class V4Prompt {
        public Caption caption { get; set; }
        public bool? use_coords { get; set; }
        public bool? use_order { get; set; }

        public V4Prompt(Caption caption) {
            this.caption = caption;
        }
    }

    class Nai3DirectorToolsBody : Nai3Body {
        public int height { get; set; }
        public int width { get; set; }
        public string image { get; set; }
        public string req_type { get; set; }
        public string prompt { get; set; }
        public int? defry { get; set; }

        public Nai3DirectorToolsBody(int height, int width, string image, string req_type, string prompt = null,
            int? defry = null) {
            this.height = height;
            this.width = width;
            this.image = image;
            this.req_type = req_type;
            this.prompt = prompt;
            this.defry = defry;
        }
    }


    internal class NovalAi {
        private string GetFileName() {
            // 获取当前时间并转换为适合作为文件名的格式
            return DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }

        private RestRequest GetRequest(string token, string path) {
            var request = new RestRequest(path, Method.Post);
            request.AddHeader("accept", "*/*");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9,en;q=0.8");
            request.AddHeader("authorization", $"Bearer {token}");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("dnt", "1");
            request.AddHeader("origin", "https://novelai.net");
            request.AddHeader("priority", "u=1, i");
            request.AddHeader("referer", "https://novelai.net/");
            request.AddHeader("sec-ch-ua",
                "\"Chromium\";v=\"124\", \"Google Chrome\";v=\"124\", \"Not-A.Brand\";v=\"99\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-site", "same-site");
            return request;
        }

        private RestClient GetClient(string proxy) {
            var options = new RestClientOptions("https://image.novelai.net") {
                Timeout = TimeSpan.FromSeconds(120),
                UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36",
                ThrowOnAnyError = true,
            };

            if (proxy != null && proxy != "")
                options.Proxy = new WebProxy(proxy);
            var client = new RestClient(options);
            return client;
        }

        private Bitmap UnZipAndSaveImage(RestResponse response, Form1 form, string prompt, string noArtistPrompt) {
            if (!response.IsSuccessful) {
                Logger.Warn($"生成失败，错误码{response.StatusCode}，错误信息{response.StatusDescription}");
                return null;
            }

            using (MemoryStream memoryStream = new MemoryStream(response.RawBytes)) {
                Tools.IsExist(form.txtOutputPath.Text, true);
                using (ZipArchive archive = new ZipArchive(memoryStream)) {
                    foreach (ZipArchiveEntry entry in archive.Entries) {
                        string file_name = GetFileName();
                        string entryFileName = form.txtOutputPath.Text + '/' + file_name;

                        using (Stream entryStream = entry.Open()) {
                            using (MemoryStream entryMemoryStream = new MemoryStream()) {
                                entryStream.CopyTo(entryMemoryStream);
                                File.WriteAllBytes(entryFileName + ".png", entryMemoryStream.ToArray());

                                if (form.picView.Image != null) {
                                    form.picView.Image.Dispose();
                                }

                                Bitmap bitmap = new Bitmap(entryMemoryStream);
                                if (form.chkSavePromptToTxt.Checked)
                                    if (form.chkSavePromptToTxtNoArtist.Checked)
                                        File.WriteAllText(entryFileName + ".txt", noArtistPrompt);
                                    else
                                        File.WriteAllText(entryFileName + ".txt", prompt);
                                return bitmap;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public Bitmap SendDirectorToolsRequests(string token, Nai3DirectorToolsBody body, Form1 form) {
            try {
                var request = GetRequest(token, "/ai/augment-image");
                request.AddStringBody(body.ToJson(), DataFormat.Json);
                var client = GetClient(form.txtProxy.Text);
                Task<RestResponse> task = client.ExecuteAsync(request);
                task.Wait();
                RestResponse response = task.Result;
                Thread.Sleep(1000);
                Bitmap pic = UnZipAndSaveImage(response, form, null, null);
                Logger.Info($"生成成功");
                return pic;
            }
            catch (Exception ex) {
                Logger.Warn($"生成失败，错误信息{ex.ToString()}");
                return null;
            }
        }

        public Bitmap SendGenerateRequests(string token, BodyBase body, string noArtistPrompt, Form1 form) {
            try {
                string data = body.ToJson();
                var request = GetRequest(token, "/ai/generate-image");
                request.AddStringBody(body.ToJson(), DataFormat.Json);
                var client = GetClient(form.txtProxy.Text);
                Task<RestResponse> task = client.ExecuteAsync(request);
                task.Wait();
                RestResponse response = task.Result;
                Thread.Sleep(1000);
                Bitmap pic = UnZipAndSaveImage(response, form, body.prompt, noArtistPrompt);
                Logger.Info("生成成功");
                return pic;
            }
            catch (Exception ex) {
                Logger.Warn($"生成失败，错误信息{ex.Message}");
                Logger.Warn($"{ex.ToString()}");
                return null;
            }
        }
    }
}