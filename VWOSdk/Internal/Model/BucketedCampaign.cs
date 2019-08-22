using System.Collections.Generic;

namespace VWOSdk
{
    internal class BucketedCampaign : Campaign
    {
        public BucketedCampaign(int id, double PercentTraffic, string Key, string Status, string Type) : base(id, PercentTraffic, Key, Status, Type, null, null)
        {

        }

        public new RangeBucket<Variation> Variations { get; set; }
        public new Dictionary<string, Goal> Goals { get; set; }
    }
}