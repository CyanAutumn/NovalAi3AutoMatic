using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Runtime.Remoting.Proxies;
using System.Net;

namespace AutoNai3Tools.utils
{
    public static class Request
    {
        public static RestResponse Post(string url, string path, string jsonBody, string token = null, string proxy = null)
        {
            var options = new RestClientOptions(url) {
                Timeout = TimeSpan.FromSeconds(120),
                UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36",
                ThrowOnAnyError = true,
            };

            if (proxy != null && proxy != "")
                options.Proxy = new WebProxy(proxy);
            var client = new RestClient(options);

            var request = new RestRequest(path, Method.Post);
            request.AddHeader("accept", "*/*");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9,en;q=0.8");
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("authorization", $"Bearer {token}");
            }
            request.AddHeader("content-type", "application/json");
            request.AddHeader("dnt", "1");
            request.AddHeader("origin", "https://novelai.net");
            request.AddHeader("priority", "u=1, i");
            request.AddHeader("referer", "https://novelai.net/");
            request.AddHeader("sec-ch-ua", "\"Not)A;Brand\";v=\"8\", \"Chromium\";v=\"138\", \"Google Chrome\";v=\"138\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-site", "same-site");
            
            request.AddStringBody(jsonBody, DataFormat.Json);
            
            var task = client.ExecuteAsync(request);
            task.Wait();
            return task.Result;
        }
    }
}