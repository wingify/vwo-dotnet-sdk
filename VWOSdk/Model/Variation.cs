using Newtonsoft.Json;

namespace VWOSdk
{
    public class Variation
    {
        [JsonConstructor]
        internal Variation(int id, string name, Changes changes, double weight)
        {
            this.Id = id;
            this.Name = name;
            this.Changes = changes;
            this.Weight = weight;
        }

        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public Changes Changes { get; internal set; }
        public double Weight { get; internal set; }
    }
}