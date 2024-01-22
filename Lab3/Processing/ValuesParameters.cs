namespace Lab3.Processing
{
    internal class ValuesParameters
    {
        private double[] _values;

        public ValuesParameters(
            double[] values, 
            int k,
            int c)
        {
            this._values = values;

            Average = EstimateErrorEngine.Average(_values);
            HalfSum = EstimateErrorEngine.HalfSum(_values);
            Median = EstimateErrorEngine.Median(_values);
            SegmentMedian = EstimateErrorEngine.SegmentAverage(_values, k);           

            AverageError = EstimateErrorEngine.RelativeEstimationError(c, Average);
            HalfSumError = EstimateErrorEngine.RelativeEstimationError(c, HalfSum);
            MedianError = EstimateErrorEngine.RelativeEstimationError(c, Median);
            SegmentMedianError = EstimateErrorEngine.RelativeEstimationError(c, SegmentMedian);

            Dispersion = EstimateErrorEngine.Dispersion(_values);
        }

        public double Average { get; } 

        public double HalfSum { get; } 

        public double Median { get; }

        public double SegmentMedian { get; }

        public double AverageError { get; }

        public double HalfSumError { get; }

        public double MedianError { get; }

        public double SegmentMedianError { get; }

        public double Dispersion { get; }

        public double AverageQuadEstimate => Math.Sqrt(Dispersion);
    }
}
