using Newtonsoft.Json;

namespace VWOSdk
{
    public class Goal
    {
        [JsonConstructor]
        internal Goal(int id, string identifier, string type)
        {
            this.Id = id;
            this.Identifier = identifier;
            this.Type = type;
        }

        public string Identifier { get; internal set; }
        public int Id { get; internal set; }
        public string Type { get; internal set; }

        internal bool IsRevenueType()
        {
            return this.Type.Equals("REVENUE_TRACKING");
        }
    }
}