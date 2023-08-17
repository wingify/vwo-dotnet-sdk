﻿#pragma warning disable 1587
/**
 * Copyright 2019-2021 Wingify Software Pvt. Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#pragma warning restore 1587

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VWOSdk
{
    internal class ApiCaller : IApiCaller
    {
        private static readonly string CUSTOM_HEADER_USER_AGENT = Constants.Visitor.CUSTOM_HEADER_USER_AGENT;
        private static readonly string CUSTOM_HEADER_IP = Constants.Visitor.CUSTOM_HEADER_IP;

        private static readonly string file = typeof(ApiCaller).FullName;

        private static HttpClient _httpClient = new HttpClient();

        public T Execute<T>(ApiRequest apiRequest)
        {
            return AsyncHelper.RunSync<T>(() => ExecuteAsync<T>(apiRequest));
        }

         public T GetJsonContent<T>()
        {
            return default(T);
        }

        public async Task<byte[]> ExecuteAsync(ApiRequest apiRequest, string visitorUserAgent = null, string userIpAddress = null)
        {
            if (apiRequest == null)
                return null;
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(apiRequest.Method.GetHttpMethod(), apiRequest.Uri.ToString());
            httpRequestMessage.Headers.Add(CUSTOM_HEADER_USER_AGENT, visitorUserAgent);
            httpRequestMessage.Headers.Add(CUSTOM_HEADER_IP, userIpAddress);
            
            HttpResponseMessage httpResponseMessage = null;
            try
            {
                httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                    LogInfoMessage.ImpressionSuccess(file, apiRequest.logUri?.ToString());
                    return response;
                }
                LogErrorMessage.ImpressionFailed(file, apiRequest.logUri?.ToString());
            }
            catch (Exception exception)
            {
                LogErrorMessage.ImpressionFailed(file, apiRequest.logUri?.ToString());
            }
            finally
            {

            }
            return null;
        }

        private async Task<T> ExecuteAsync<T>(ApiRequest apiRequest, string visitorUserAgent = null, string userIpAddress = null)
        {
            try {
                var responseContent = await ExecuteAsync(apiRequest, visitorUserAgent, userIpAddress);
                return Deserialize<T>(responseContent);
            }
            catch(Exception exception)
            {
            }
            return default(T);
        }

        private static T Deserialize<T>(byte[] byteContent)
        {
            if (byteContent != null)
            {
                var json = Encoding.UTF8.GetString(byteContent);
                if (typeof(T) == typeof(Settings)) {
                    // As C# requires goals, campaigns and variables to be list
                    // Replace empty structure of {} to []
                    json = json.Replace("\"goals\":{}", "\"goals\":[]");
                    json = json.Replace("\"campaigns\":{}", "\"campaigns\":[]");
                    json = json.Replace("\"variables\":{}", "\"variables\":[]");
                }
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            return default(T);
        }


    }
}
