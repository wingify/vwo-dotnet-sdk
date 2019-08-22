using System.Collections.Generic;

namespace VWOSdk
{
    internal class AccountSettings : Settings
    {
        public AccountSettings(string sdkKey, List<BucketedCampaign> campaigns, int accountId, int version): base(sdkKey, null, accountId, version)
        {
            this.Campaigns = campaigns;
        }

        public new List<BucketedCampaign> Campaigns { get; set; }
    }
}