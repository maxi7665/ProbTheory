using ScottPlot;
using System.Reflection;

namespace Core
{
    public class PlotDescriptor
    {
        public List<PlotSegment> Segments { get; set; } = new();

        public double MinX { get; set; } = 0;

        public double MaxX { get; set; } = 1;

        public List<Coordinates> GetCoords(double fromX, double toX, double step = 0.01)
        {
            var ret = new SortedList<double, Coordinates>();

            foreach (var segment in Segments)
            {
                var segmentFromX = Math.Max(fromX, segment.MinX);
                var segmentToX = Math.Min(toX, segment.MaxX);

                double x = segmentFromX;

                for (int i = 0; x <= segmentToX ; i++)
                {
                    x = segmentFromX + (i * step);

                    var coords = new Coordinates(x, segment.Func(x));

                    ret.Add(x, coords);
                }
            }

            return ret.ToList().ConvertAll(v => v.Value);
        }
    }
}
