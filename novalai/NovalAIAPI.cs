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
        public static string GetVibeID(string base64Image, float information_extracted, string model, string token, string api = null) {
            try {
                var requestBody = new VibeRequest ( base64Image, information_extracted, model );

                var json = JsonConvert.SerializeObject(requestBody);
                string baseUrl = ApiEndpoint.ResolveBaseUrl(api);
                var response = Request.Post(baseUrl, "/ai/encode-vibe", json, token);

                if (response.IsSuccessStatusCode) {
                    var vibeBytes = response.RawBytes;
                    string base64Vibe = Convert.ToBase64String(vibeBytes);

                    Logger.Info("Vibe 编码成功",
                        context: Logger.Context(("model", model), ("informationExtracted", information_extracted)));
                    return base64Vibe;
                }

                Logger.Error("Vibe 编码失败",
                    context: Logger.Context(("statusCode", response.StatusCode),
                        ("description", response.StatusDescription),
                        ("model", model),
                        ("informationExtracted", information_extracted),
                        ("responseBody", response.Content)));
                return null;
            }
            catch (Exception ex) {
                Logger.Error("Vibe 编码请求异常", exception: ex,
                    context: Logger.Context(("model", model), ("informationExtracted", information_extracted)));
                return null;
            }
        }

    }
}
