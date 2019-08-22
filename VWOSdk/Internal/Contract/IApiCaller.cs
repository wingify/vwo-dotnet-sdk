using System.Threading.Tasks;

namespace VWOSdk
{
    internal interface IApiCaller
    {
        T Execute<T>(ApiRequest apiRequest);
        Task<byte[]> ExecuteAsync(ApiRequest apiRequest);
    }
}