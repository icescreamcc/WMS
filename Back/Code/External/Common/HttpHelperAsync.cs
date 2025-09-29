using External.Log;
using Models.Model.Sys;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace External.Common
{
    /// <summary>
    /// http请求异步版
    /// </summary>
    public class HttpHelperAsync
    {
        private readonly LogHelper _logHelper;

        public HttpHelperAsync(LogHelper logHelper)
        {
            _logHelper = logHelper;
        }

        public  async Task<TResponse> RequestGetAsync<TResponse>(string url, Dictionary<string, string> headers = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var dic in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(dic.Key, dic.Value);
                    }
                }
                httpClient.Timeout = new TimeSpan(0, 0, 2);
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TResponse>(content); 
                }
                else
                {
                    _logHelper.LogError("RequestGet", $"GET请求失败，错误码：{(int)response.StatusCode},响应体：{content}", url, null, null);
                    return default;
                }
            }
        }

        public  async Task<TResponse> RequestPostAsync<TRequest, TResponse>(TRequest input, string url, Dictionary<string, string> headers = null)
        { 
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var httpClient = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var dic in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(dic.Key, dic.Value);
                    }
                }
                httpClient.Timeout = new TimeSpan(0, 0, 12);
                var response = await httpClient.PostAsJsonAsync(url, input);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<TResponse>(content);
                else
                {
                    _logHelper.LogError("RequestPostAsync", $"POST请求失败，错误码：{(int)response.StatusCode},响应体：{content}", url, null, null);
                    return default;
                }
            }
        }

        public  TResponse RequestGet<TResponse>(string url, Dictionary<string, string> headers = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var dic in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(dic.Key, dic.Value);
                    }
                }
                httpClient.Timeout = new TimeSpan(0, 0, 2);
                var response =  httpClient.GetAsync(url).Result;
                var content =  response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TResponse>(content);

                }
                else
                {
                    _logHelper.LogError("RequestGet", $"GET请求失败，错误码：{(int)response.StatusCode},响应体：{content}", url, null, null);
                    return default;
                }
            }
        }

        public  TResponse RequestPost<TRequest, TResponse>(TRequest input, string url, Dictionary<string, string> headers = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var dic in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(dic.Key, dic.Value);
                    }
                }
                httpClient.Timeout = new TimeSpan(0, 0, 2);
                var response =  httpClient.PostAsJsonAsync(url, input).Result;
                var content =  response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<TResponse>(content);
                else
                {
                    _logHelper.LogError("RequestPost", $"POST请求失败，错误码：{(int)response.StatusCode},响应体：{content}", url, null, null);
                    return default;
                }
            }
        }
         
    }
}
