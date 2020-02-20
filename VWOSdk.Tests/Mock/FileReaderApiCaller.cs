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

using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VWOSdk.Tests
{
    internal class FileReaderApiCaller : IApiCaller
    {
        private string _settingsFileName;

        public FileReaderApiCaller()
        {
            _settingsFileName = "SampleSettingsFile";
        }

        public FileReaderApiCaller(string settingsFileName)
        {
            _settingsFileName = settingsFileName;
        }

        public T Execute<T>(ApiRequest apiRequest)
        {
            if(typeof(T) == typeof(Settings) || typeof(T) == typeof(Dictionary<string, dynamic>))
            {
                return GetSettingsFile<T>(this._settingsFileName);
            }
            return default(T);
        }
        public T GetJsonContent<T>()
        {
            if(typeof(T) == typeof(Settings) || typeof(T) == typeof(Dictionary<string, dynamic>))
            {
                return GetSettingsFile<T>(this._settingsFileName);
            }
            return default(T);
        }

        private T GetSettingsFile<T>(string filename)
        {
            string json = GetJsonText(filename);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        private string GetJsonText(string filename)
        {
            string path = null;

            foreach (var resource in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (resource.Contains("." + filename + "."))
                {
                    path = resource;
                    break;
                }
            }

            try
            {
                var _assembly = Assembly.GetExecutingAssembly();
                using (Stream resourceStream = _assembly.GetManifestResourceStream(path))
                {
                    if (resourceStream == null)
                        return null;

                    using (StreamReader reader = new StreamReader(resourceStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch { }

            return null;
        }

        public Task<byte[]> ExecuteAsync(ApiRequest apiRequest)
        {
            return Task.FromResult(new byte[16]);
        }
    }
}
