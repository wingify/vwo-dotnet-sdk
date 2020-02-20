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

namespace VWOSdk
{
    internal class AppContext
    {
        private static readonly string file = typeof(AppContext).FullName;

        internal static IApiCaller ApiCaller { get; set; } = new ApiCaller();
        internal static ILogWriter Logger { get; set; } = new DefaultLogWriter();
        internal static LogLevel LogLevel { get; set; } = LogLevel.ERROR;

        internal static void Configure(IApiCaller apiCaller)
        {
            ApiCaller = apiCaller;
        }

        internal static void Configure(ILogWriter logger)
        {
            if (logger != null)
            {
                Logger = logger;
                LogDebugMessage.CustomLoggerUsed(file);
            }
            else
            {
                Logger = new DefaultLogWriter();
                LogErrorMessage.CustomLoggerMisconfigured(file);
            }
        }

        internal static void Configure(LogLevel logLevel)
        {
            LogLevel = logLevel;
            LogDebugMessage.LogLevelSet(file, logLevel);
        }
    }
}
