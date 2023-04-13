using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Plotting : MonoBehaviour
{
    private string[] minCoords;
    private string[] maxCoords;

    private double[,] heatmapData;

    private string[] lines;

    public void ReadAndPlot(string dataFilePath)
    {
        //reads file
        lines = File.ReadAllLines(dataFilePath);

        //first two lines are min and max coordinates
        minCoords = lines[0].Replace("(", "").Replace(")", "").Replace("\n", "").Split(',');
        maxCoords = lines[1].Replace("(", "").Replace(")", "").Replace("\n", "").Split(',');

        //sets the heatmap array size from min and max coordinates
        int heatmapSizeX = (int)(double.Parse(maxCoords[0]) * 10 - double.Parse(minCoords[0]) * 10);
        int heatmapSizeY = (int)(double.Parse(maxCoords[1]) * 10 - double.Parse(minCoords[1]) * 10);
        Debug.Log(heatmapSizeX);
        Debug.Log(heatmapSizeY);
        heatmapData = new double[heatmapSizeX+1, heatmapSizeY+1];
    }

    public void CreateHeatmap(string filePath)
    {
        var model = new PlotModel { Title = "Heatmap" };
        // Color axis (the X and Y axes are generated automatically)
        model.Axes.Add(new LinearColorAxis
        {
            Palette = OxyPalettes.Rainbow(100)
        });


        int xOffset = (int)Math.Abs(double.Parse(minCoords[0]) * 10);
        int yOffset = (int)Math.Abs(double.Parse(minCoords[1]) * 10);
        Debug.Log(xOffset);
        Debug.Log(yOffset);
        for (int i = 2; i < lines.Length; i++)
        {
            //normalizes the data so it fits the heatmap bin array, +1 if the data point falls into the bin
            var values = lines[i].Replace("(", "").Replace(")", "").Replace("\n", "").Split(',');
            double x = double.Parse(values[0]) * 10;
            double y = double.Parse(values[1]) * 10;
            heatmapData[(int)x + xOffset, (int)y + yOffset] += 1;
        }

        // adds the heatmap bin array to the graph and sets the axis
        var heatMapSeries = new HeatMapSeries
        {
            X0 = (int)double.Parse(minCoords[0]),
            X1 = (int)double.Parse(maxCoords[0]),
            Y0 = (int)double.Parse(minCoords[1]),
            Y1 = (int)double.Parse(maxCoords[1]),
            Interpolate = true,
            RenderMethod = HeatMapRenderMethod.Bitmap,
            Data = heatmapData
        };

        model.Series.Add(heatMapSeries);
        WriteToSVG(model, filePath);
    }

    public void CreateScatterPlot(string filePath)
    {
        var model = new PlotModel { Title = "Scatterplot", PlotType = PlotType.XY };
        var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };

        //Adds data points to the scatter graph
        for (int i = 2; i < lines.Length; i++)
        {
            var values = lines[i].Replace("(", "").Replace(")", "").Replace("\n", "").Split(',');
            scatterSeries.Points.Add(new ScatterPoint(double.Parse(values[0]), double.Parse(values[1]), 1, 1000));
        }
        model.Series.Add(scatterSeries);
        model.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Minimum = double.Parse(minCoords[0]), Maximum = double.Parse(maxCoords[0]), Key = "Horizontal" });
        model.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Minimum = double.Parse(minCoords[1]), Maximum = double.Parse(maxCoords[1]), Key = "Vertical" });
        scatterSeries.XAxisKey = "Horizontal";
        scatterSeries.YAxisKey = "Vertical";
        WriteToSVG(model,filePath);
        Debug.Log("Scatterplot created");
    }

    public void WriteToSVG(PlotModel model, string newFilePath)
    {
        using (var stream = File.Create(newFilePath + ".svg"))
        {
            var exporter = new SvgExporter { Width = 600, Height = 400 };
            exporter.Export(model, stream);
        }
    }
    
}
