using Newtonsoft.Json;
using System.Collections.Generic;

namespace VWOSdk
{
    public class Settings
    {
        [JsonConstructor]
        internal Settings(string sdkKey, List<Campaign> campaigns, int accountId, int version)
        {
            this.SdkKey = sdkKey;
            this.Campaigns = campaigns;
            this.AccountId = accountId;
            this.Version = version;
        }

        public string SdkKey { get; internal set; }
        public IReadOnlyList<Campaign> Campaigns { get; internal set; }
        public int AccountId { get; internal set; }
        public int Version { get; internal set; }
    }
}