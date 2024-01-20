// See https://aka.ms/new-console-template for more information
using Core;
using ScottPlot;

Plot densityPlot = new();

List<string> fileNamesToOpen = new();


var plotDescriptor = new PlotDescriptor();

// функция плотности распределения вероятностей
Func<double, double> probabilityDensityFunction = x =>
{
    if (x >= 0 && x <= 1)
    {
        return (0.625 * (x + 1.1));
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

var fileName = "probabilityDensityFunction.png";

densityPlot.Legend.IsVisible = true;

densityPlot.SavePng(fileName, 800, 600);

Utils.OpenPath(fileName);

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

distributionScottPlot.Legend.IsVisible = true;

var distribFileName = "distribPlot.png";

distributionScottPlot.SavePng(distribFileName, 600, 400);

Utils.OpenPath(distribFileName);

// вывод моды
Console.WriteLine($"Мода: {probabilityDensityFunction(1)}");