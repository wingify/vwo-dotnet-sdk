namespace VWOSdk
{
    internal class WeightRange
    {
        public WeightRange(double start, double end)
        {
            this.Start = start;
            this.To = end;
        }

        public double Start { get; set; }
        public double To { get; set; }

        public override bool Equals(object obj)
        {
            var range = obj as WeightRange;
            return range != null &&
                   Start == range.Start &&
                   To == range.To;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = -1781160927;
                hashCode = hashCode * -1521134295 + Start.GetHashCode();
                hashCode = hashCode * -1521134295 + To.GetHashCode();
                return hashCode;
            }
        }

        public bool IsInRange(double value)
        {
            return this.Start <= value && this.To >= value;
        }
    }
}