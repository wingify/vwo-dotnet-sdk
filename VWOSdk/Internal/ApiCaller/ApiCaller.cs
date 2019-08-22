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
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            return default(T);
        }
    }
}