#pragma warning disable 1587
/**
 * Copyright 2019-2020 Wingify Software Pvt. Ltd.
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

        public async Task<byte[]> ExecuteAsync(ApiRequest apiRequest)
        {
            if (apiRequest == null)
                return null;
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(apiRequest.Method.GetHttpMethod(), apiRequest.Uri.ToString());
            HttpResponseMessage httpResponseMessage = null;
            try
            {
                httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                    LogInfoMessage.ImpressionSuccess(file, apiRequest.Uri.ToString());
                    return response;
                }
                LogErrorMessage.ImpressionFailed(file, apiRequest.Uri?.ToString());
            }
            catch (Exception exception)
            {
                LogErrorMessage.ImpressionFailed(file, apiRequest.Uri?.ToString());
            }
            finally
            {

            }
            return null;
        }

        private async Task<T> ExecuteAsync<T>(ApiRequest apiRequest)
        {
            try {
                var responseContent = await ExecuteAsync(apiRequest);
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
