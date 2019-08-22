using System.Collections.Generic;
using System.Net.Http;

namespace VWOSdk
{
    internal static class Extensions
    {
        private static Dictionary<Method, HttpMethod> HttpMethodMap = new Dictionary<Method, HttpMethod>
        {
            { Method.GET, HttpMethod.Get },
        };

        public static HttpMethod GetHttpMethod(this Method method)
        {
            return HttpMethodMap[method];
        }

        public static bool IsLogTypeEnabled(this LogLevel specifiedLogLevel, LogLevel logLevel)
        {
            return logLevel <= specifiedLogLevel;
        }
    }
}
