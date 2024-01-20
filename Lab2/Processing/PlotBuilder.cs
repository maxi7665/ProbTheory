using ScottPlot;

namespace Lab2.Processing
{
    internal class PlotBuilder
    {
        public static Plot BuildGistogram(
            SegmentStatistics segmentStatistics)
        {
            var plot = new Plot();
            var bars = new List<Bar>();
            var ticks = new List<Tick>();

            int cnt = 0;

            foreach (var (from, part) in segmentStatistics.Parts)
            {
                cnt++;

                var x = part.From + (part.To - part.From) / 2;

                var bar = new Bar()
                {
                    Position = cnt,
                    Value = part.Values.Length,
                    ValueBase = 0,
                    Orientation = Orientation.Vertical,
                    //Size = 0
                };

                ticks.Add(new(cnt, cnt.ToString() + "\r\n" + bar.Value.ToString()));

                bars.Add(bar);
            }

            plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks.ToArray());
            plot.Axes.Bottom.MajorTickStyle.Length = 0;
            plot.HideGrid();

            // tell the plot to autoscale with no padding beneath the bars
            plot.Axes.Margins(bottom: 0);

            var barsPlot = plot.Add.Bars(bars);

            return plot;
        }
    }
}
