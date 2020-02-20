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
