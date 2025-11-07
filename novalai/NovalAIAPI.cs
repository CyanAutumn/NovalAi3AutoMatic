using AutoNai3Tools.body;
using AutoNai3Tools.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.novalai
{
    class NovalAIAPI
    {
        private const string VIBE_ENCODE_URL = "https://image.novelai.net";

        public static string GetVibeID(string base64Image, float information_extracted, string model,string token) {
            try {
                var requestBody = new VibeRequest ( base64Image, information_extracted, model );

                var json = JsonConvert.SerializeObject(requestBody);
                var response = Request.Post(VIBE_ENCODE_URL, "/ai/encode-vibe", json, token);

                if (response.IsSuccessStatusCode) {
                    // The response is raw binary data (the vibe signature).
                    var vibeBytes = response.RawBytes;

                    // The vibe data must be Base64 encoded to be used in the next request.
                    string base64Vibe = Convert.ToBase64String(vibeBytes);

                    Logger.Info($"Vibe编码成功");
                    return base64Vibe;
                }
                else {
                    Logger.Error($"Vibe编码失败: {response.StatusCode} - {response.StatusDescription}");
                    var errorContent = response.Content;
                    Logger.Error($"错误详情: {errorContent}");
                    return null;
                }
            }
            catch (Exception ex) {
                Logger.Error($"Vibe编码异常: {ex.Message}");
                return null;
            }
        }

    }
}
