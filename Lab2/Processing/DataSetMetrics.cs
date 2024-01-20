using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Processing
{
    internal class DataSetMetrics
    {
        public double R { get; set; }
        public double ExpectValue { get; internal set; }
        public double Mode { get; internal set; }
        public double Median { get; internal set; }

        public Dictionary<int, SegmentStatistics> QSegmentStatistics { get; set; } = new();
        public double D { get; internal set; }

        public double Sigma => Math.Sqrt(D);
    }
}
