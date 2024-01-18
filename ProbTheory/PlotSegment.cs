namespace Core
{
    /// <summary>
    /// Segment of plot
    /// </summary>
    public class PlotSegment
    {
        public Func<double, double> Func { get; set; } = (x) => x;

        public double MinX { get; set; } = double.MinValue;

        public double MaxX { get; set; } = double.MaxValue;

        public double Step { get; set; } = 0.01;

        public List<double> GetXCoords()
        {
            var ret = new List<double>();            

            for (double i = MinX; i <= MaxX; i += Step)
            {
                ret.Add(i);
            }

            return ret;
        }

        public List<double> GetYCoords()
        {
            var ret = GetXCoords().ConvertAll(x => Func(x));

            return ret;
        }

        public bool HasMapping(double x) => x <= MaxX && x >= MinX;
    }
}
