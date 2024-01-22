// See https://aka.ms/new-console-template for more information
using Core;
using ScottPlot;

internal class Lab1
{
    private static void Main(string[] args)
    {
        // плотность распределения
        var densityPlot = CreateDensityScatter();

        var fileName = "probabilityDensityFunction.png";

        densityPlot.Legend.IsVisible = true;

        densityPlot.SavePng(fileName, 800, 600);

        Utils.OpenPath(fileName);

        // распределение 
        var distributionPlot = CreateDistributionScatter();

        distributionPlot.Legend.IsVisible = true;

        var distribFileName = "distribPlot.png";

        distributionPlot.SavePng(distribFileName, 800, 600);

        Utils.OpenPath(distribFileName);


        AddAdditionalScatters(densityPlot);
        AddAdditionalScatters(distributionPlot);

        densityPlot.Legend.Location = Alignment.LowerCenter;
        distributionPlot.Legend.Location = Alignment.LowerCenter;

        var fullDensityFileName = "fullDensity.png";

        densityPlot.SavePng(fullDensityFileName, 800, 600);

        Utils.OpenPath(fullDensityFileName);

        var fullDistributionFileName = "fullDistribution.png";

        distributionPlot.SavePng(fullDistributionFileName, 800, 600);

        Utils.OpenPath(fullDistributionFileName);
    }

    private static Plot CreateDensityScatter()
    {
        Plot densityPlot = new();

        var plotDescriptor = new PlotDescriptor();

        // функция плотности распределения вероятностей
        Func<double, double> probabilityDensityFunction = x =>
        {
            if (x >= 0 && x <= 1)
            {
                return 0.625 * (x + 1.1);
            }

            return 0;
        };

        var plotSegment = new PlotSegment()
        {
            MinX = -0.1,
            MaxX = 1.1,
            Func = probabilityDensityFunction
        };

        plotDescriptor.Segments.Add(plotSegment);

        var values = plotDescriptor.GetCoords(-0.1, 1.1, 0.01);

        var scatter = densityPlot.Add.Scatter(
            values);

        scatter.LineStyle.Width = 2;
        scatter.MarkerStyle.Size = 5;

        scatter.Label = "График плотности распределения вероятностей";

        // вывод моды
        Console.WriteLine($"Мода: {probabilityDensityFunction(1)}");

        return densityPlot;                      
    }

    private static Plot CreateDistributionScatter()
    {
        // функция распределения
        Func<double, double> distributionFunction = x =>
        {
            if (x < 0)
            {
                return 0;
            }
            else if (x > 1)
            {
                return 1;
            }

            return 0.625 * (Math.Pow(x, 2) / 2 + 1.1 * x);
        };

        var distributionPlotSegment = new PlotSegment()
        {
            MinX = -0.1,
            MaxX = 1.1,
            Func = distributionFunction
        };

        var distributionPlot = new PlotDescriptor();

        distributionPlot.Segments.Add(distributionPlotSegment);


        Plot distributionScottPlot = new();

        var distributionScatter = distributionScottPlot.Add
            .Scatter(distributionPlot.GetCoords(-0.1, 1.1));

        distributionScatter.Label = "Функция распределения";

        return distributionScottPlot;        
    }

    private const double expectedValue = 0.552;
    private const double medianValue = 0.576305;
    private const double modeValue = 1;

    private static void AddAdditionalScatters(Plot plot)
    {
        var expected = plot.Add.VerticalLine(
            expectedValue, 
            color: Color.FromARGB((uint)System.Drawing.Color.Red.ToArgb()));

        var median = plot.Add.VerticalLine(
            medianValue,
            color: Color.FromARGB((uint)System.Drawing.Color.Green.ToArgb()));

        var mode = plot.Add.VerticalLine(
           modeValue,
           color: Color.FromARGB((uint)System.Drawing.Color.Purple.ToArgb()));

        expected.Text = $"Мат. ожидание {expectedValue}";
        expected.Label.IsVisible = false;

        median.Text = $"Медиана {medianValue}";
        median.Label.IsVisible = false;

        mode.Text = $"Мода {modeValue}";
        mode.Label.IsVisible = false;
    }
}