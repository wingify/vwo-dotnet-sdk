using System.IO;
using System.Reflection;
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
            if(typeof(T) == typeof(Settings))
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