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
