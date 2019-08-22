using Newtonsoft.Json;
using System.Collections.Generic;

namespace VWOSdk
{
    public class Campaign
    {
        [JsonConstructor]
        internal Campaign(int id, double PercentTraffic, string Key, string Status, string Type, List<Goal> goals, List<Variation> variations)
        {
            this.PercentTraffic = PercentTraffic;
            this.Key = Key;
            this.Status = Status;
            this.Type = Type;
            this.Id = id;
            this.Goals = goals;
            this.Variations = variations;
        }

        public IReadOnlyList<Goal> Goals { get; internal set; }
        public IReadOnlyList<Variation> Variations { get; internal set; }
        public int Id { get; internal set; }
        public double PercentTraffic { get; internal set; }
        public string Key { get; internal set; }
        public string Status { get; internal set; }
        public string Type { get; internal set; }
    }
}