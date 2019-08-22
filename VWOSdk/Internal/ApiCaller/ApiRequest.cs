using System;

namespace VWOSdk
{
    internal enum Method
    {
        GET = 0
    }
    
    internal class ApiRequest
    {
        private readonly bool _isDevelopmentMode;

        public ApiRequest(Method method, bool isDevelopmentMode = false)
        {
            this.Method = method;
            this._isDevelopmentMode = isDevelopmentMode;
        }

        public Method Method { get; private set; }
        public Uri Uri { get; set; }
        public IApiCaller ApiCaller { get; private set; }

        public ApiRequest WithCaller(IApiCaller apiCaller)
        {
            this.ApiCaller = apiCaller;
            return this;
        }

        public void ExecuteAsync()
        {
            if (this._isDevelopmentMode)
                return;
            this.ApiCaller.ExecuteAsync(this);
        }

        public T Execute<T>()
        {
            if(this._isDevelopmentMode)
                return default(T);
            return this.ApiCaller.Execute<T>(this);
            
        }
    }
}