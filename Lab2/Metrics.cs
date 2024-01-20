using Lab2.Processing;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace Lab2
{
    internal class Metrics
    {
        public static DataSetMetrics CountMetrics(
            double[] values,
            int[] qValues)
        {
            var ret = new DataSetMetrics();

            ret.R = values.Max() - values.Min();

            ret.ExpectValue = values.Average();

            var sortedValues = values.ToList();
            
            sortedValues.Sort();

            if (sortedValues.Count % 2 == 0)
            {
                ret.Median = (sortedValues[sortedValues.Count / 2 - 1]
                    + sortedValues[sortedValues.Count / 2]) / 2;
            }
            else
            {
                ret.Median = sortedValues[sortedValues.Count / 2 - 1];
            }

            foreach (var q in qValues)
            {
                var statistics = BuildSegmentStatistics(values, q);

                ret.QSegmentStatistics.Add(q, statistics);
            }

            if (ret.QSegmentStatistics.Count != 0)
            {
                var elderStatistics = ret.QSegmentStatistics
                    .MaxBy(p => p.Key);

                var mainSegmentValues = elderStatistics.Value.Parts
                    .MaxBy(p => p.Value.Values.Length)
                    .Value.Values;

                if (mainSegmentValues.Length != 0)
                {
                    ret.Mode = mainSegmentValues.Order().ToArray()[mainSegmentValues.Length / 2];
                }                                   
            }

            ret.D = values
                .Select(v => Math.Pow(v - ret.ExpectValue, 2))
                .Sum() / (values.Length - 1);

            return ret;
        }

        private static SegmentStatistics BuildSegmentStatistics(
            double[] values,
            double q)
        {
            var max = values.Max();
            var min = values.Min();
            var r = max - min;

            var delta = r / q;

            var ret = new SegmentStatistics();

            for (int i = 1; i <= q; i ++)
            {
                var from = min + (i - 1) * delta;
                var to = min + i * delta;

                var part = new SegmentStatisticsPart();

                part.From = from;
                part.To = to;

                part.Values = values
                    .Where(v => v >= from 
                    && (v < to
                    || v == to
                    && i == q))
                    .ToArray();

                ret.Parts.Add(part.From, part);
            }

            ret.From = min;
            ret.To = max;

            return ret;
        }
    }
}
