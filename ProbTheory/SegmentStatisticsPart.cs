namespace Core
{
    public class SegmentStatisticsPart
    {
        public double From { get; set; }

        public double To { get; set; }

        public double[] Values { get; set; } = Array.Empty<double>();
    }
}
