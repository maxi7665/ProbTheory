namespace Core
{
    /// <summary>
    /// эмпирическая статистика для построения гистограммы и вычисления моды
    /// </summary>
    public class SegmentStatistics
    {
        public int PartsNumber { get => Parts.Count; }

        public double From { get; set; }

        public double To { get; set; }

        public SortedList<double, SegmentStatisticsPart> Parts { get; set; } = new();

        public int ValuesCount => Parts.Sum(p => p.Value.Values.Length);
    }
}
