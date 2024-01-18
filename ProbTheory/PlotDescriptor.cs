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

                for (double i = segmentFromX; i <= segmentToX; i += step)
                {
                    var coords = new Coordinates(i, segment.Func(i));

                    ret.Add(i, coords);
                }
            }

            return ret.ToList().ConvertAll(v => v.Value);
        }
    }
}
