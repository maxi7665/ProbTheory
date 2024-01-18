// See https://aka.ms/new-console-template for more information
using Core;
using ScottPlot;

Console.WriteLine("Hello, World!");

Plot myPlot = new();


var plotDescriptor = new PlotDescriptor();

var plotSegment = new PlotSegment()
{
    MinX = 0,
    MaxX = 1
};

plotDescriptor.Segments.Add(plotSegment);


plotDescriptor.Segments.Add(new PlotSegment()
{
    Func = (x) => 0,
    MaxX = -0.01
});

plotDescriptor.Segments.Add(new PlotSegment()
{
    Func = (x) => 0,
    MinX = 1.01
});

var values = plotDescriptor.GetCoords(-0.1, 1.1);

var scatter = myPlot.Add.Scatter(
    values);

scatter.LineStyle.Width = 5;
scatter.MarkerStyle.Size = 5;

var fileName = "demo.png";

myPlot.SavePng(fileName, 800, 600);

Utils.OpenPath(fileName);
