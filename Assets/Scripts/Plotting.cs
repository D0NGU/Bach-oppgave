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
    /* Colors meaning:
     * green = full object, seen
     * light blue = right side object, seen
     * dark blue = left side object, seen
     * yellow = 
     * light_orange = right side object, not seen
     * dark_orange = left side object, not seen
     * red = full object, not seen
     */

    /* Object types:
     * "fullsphere"
     * "righthalf"
     * "lefthalf"
     */
    private IDictionary<string, int> colorDict = new Dictionary<string, int>()
    {
        { "dark_blue", 0 },
        { "light_blue", 25 },
        { "green", 36 },
        { "yellow", 50 },
        { "light_orange", 65 },
        { "dark_orange", 85 },
        { "red", 100 },
    };
    private SaveResultsScript saveObj = new();

    private string[] minCoords;
    private string[] maxCoords;

    private double[,] heatmapData;

    private string[] lines;

    /*
     * method that extracts the size of the graph
     */
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

        for (int i = 2; i < lines.Length; i++)
        {
            if (!lines[i].Contains("Wavenumber"))
            {
                //normalizes the data so it fits the heatmap bin array, +1 if the data point falls into the bin
                var values = lines[i].Replace("(", "").Replace(")", "").Replace("\n", "").Split(',');
                double x = double.Parse(values[0]) * 10;
                double y = double.Parse(values[1]) * 10;
                heatmapData[(int)x + xOffset, (int)y + yOffset] += 1;
            }
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
            if (!lines[i].Contains("Wavenumber"))
            {
                var values = lines[i].Replace("(", "").Replace(")", "").Replace("\n", "").Split(',');
                scatterSeries.Points.Add(new ScatterPoint(double.Parse(values[0]), double.Parse(values[1]), 1, 1000));
            }
        }
        model.Series.Add(scatterSeries);
        model.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Minimum = double.Parse(minCoords[0]), Maximum = double.Parse(maxCoords[0]), Key = "Horizontal" });
        model.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Minimum = double.Parse(minCoords[1]), Maximum = double.Parse(maxCoords[1]), Key = "Vertical" });
        scatterSeries.XAxisKey = "Horizontal";
        scatterSeries.YAxisKey = "Vertical";
        WriteToSVG(model,filePath);
        Debug.Log("Scatterplot created");
    }

    public void CreateTestObjectPlot(string saveFilePath, string graphFilePath)
    {
        //loads data from json file
        ResultsDataListClass resultsList = saveObj.LoadFromJSON(graphFilePath);

        var model = new PlotModel { Title = "ScatterSeries", PlotType = PlotType.XY };
        var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };

        //an axes for different color orbs
        model.Axes.Add(new LinearColorAxis() { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100, Palette = OxyPalettes.Jet(200), IsAxisVisible = false });

        //x and y axes
        model.Axes.Add(new LinearAxis()
        {
            Position = AxisPosition.Bottom,
            Minimum = -4.5,
            Maximum = 4.5,
            Key = "Horizontal",
            Title = "\nLine = Movement to an object\n\n" +
                    "Green orb = Positions where a full object has been seen\n\n" +
                    "Ligth blue orb = Positions where a rigth side object has been seen\n\n" +
                    "Dark blue orb = Positions where a left side object has been seen\n\n" +
                    "Ligth orange = A rigth side object that has not been seen\n\n" +
                    "Dark orange = A left side object that has not been seen\n\n" +
                    "Red = A full object that has not been seen"
        });
        model.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Minimum = -0.6, Maximum = 4, Key = "Vertical" });
        scatterSeries.XAxisKey = "Horizontal";
        scatterSeries.YAxisKey = "Vertical";
        foreach (SelectableObjectResultsDataClass results in resultsList.selectableObjectsResultsDataList)
        {
            //if object of any type has movemnt
            if (results.hasMovement)
            {
                //draws a line between start position and end position
                var lineSeries = new LineSeries { };
                lineSeries.Points.Add(new DataPoint(results.startPosistion.x, results.startPosistion.y));
                lineSeries.Points.Add(new DataPoint(results.endPosistion.x, results.endPosistion.y));
                lineSeries.XAxisKey = "Horizontal";
                lineSeries.YAxisKey = "Vertical";
                model.Series.Add(lineSeries);
            }
            //if object of any type has been seen
            if (results.hasBeenSeen)
            {
                
                //swtich statement for each object type to change the color of sphere on the graph, seen
                switch (results.objectType)
                {
                    case "fullsphere":
                        scatterSeries.Points.Add(new ScatterPoint(results.positionWhenSeen.x, results.positionWhenSeen.y, results.scale.x * 20, colorDict["green"]));
                        break;
                    case "righthalf":
                        scatterSeries.Points.Add(new ScatterPoint(results.positionWhenSeen.x, results.positionWhenSeen.y, results.scale.x * 20, colorDict["light_blue"]));
                        break;
                    case "lefthalf":
                        scatterSeries.Points.Add(new ScatterPoint(results.positionWhenSeen.x, results.positionWhenSeen.y, results.scale.x * 20, colorDict["dark_blue"]));
                        break;
                }
            }
            //object of any type has not been seen
            else
            {
                //swtich statement for each object type to change the color of sphere on the graph, not seen
                switch (results.objectType)
                {
                    case "fullsphere":
                        scatterSeries.Points.Add(new ScatterPoint(results.startPosistion.x, results.startPosistion.y, results.scale.x * 20, colorDict["red"]));
                        break;
                    case "righthalf":
                        scatterSeries.Points.Add(new ScatterPoint(results.startPosistion.x, results.startPosistion.y, results.scale.x * 20, colorDict["light_orange"]));
                        break;
                    case "lefthalf":
                        scatterSeries.Points.Add(new ScatterPoint(results.startPosistion.x, results.startPosistion.y, results.scale.x * 20, colorDict["dark_orange"]));
                        break;
                }
            }
        }
        model.Series.Add(scatterSeries);
        WriteToSVG(model, saveFilePath);
    }

    public void WriteToSVG(PlotModel model, string newFilePath)
    {
        using (var stream = File.Create(newFilePath + ".svg"))
        {
            var exporter = new SvgExporter { Width = 1920, Height = 1380 };
            exporter.Export(model, stream);
        }
    }
    
}
