using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// Handles plottng from different result files.
/// Plots included are heatmap of gaze dots, scatter plot of gaze dots
/// and a plot of all objects, movement and where they have been seen
/// </summary>
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

    /// <summary>
    /// Bin array used for heatmap plotting
    /// </summary>
    private double[,] heatmapData;

    /// <summary>
    /// Every line of gaze point file
    /// </summary>
    private string[] lines;

    /// <summary>
    /// Method that extracts the size of the testing area and uses it to limit the graph
    /// </summary>
    /// <param name="dataFilePath">Full path where the gaze dots file is located</param>
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

    /// <summary>
    /// Method that handles the gaze point data and plots it as a heatmap
    /// </summary>
    /// <param name="filePath">File path where the heatmap will be created + the name of the file at the end of the path</param>
    public void CreateHeatmap(string filePath)
    {
        var model = new PlotModel { Title = "Heatmap" };
        // Color axis (the X and Y axes are generated automatically)
        model.Axes.Add(new LinearColorAxis
        {
            Palette = OxyPalettes.Rainbow(100)
        });

        // calculates the offsets of x and y axis so the data points correspond with the bin array
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
            X0 = double.Parse(minCoords[0]),
            X1 = double.Parse(maxCoords[0]),
            Y0 = double.Parse(minCoords[1]),
            Y1 = double.Parse(maxCoords[1]),
            Interpolate = true,
            RenderMethod = HeatMapRenderMethod.Bitmap,
            Data = heatmapData
        };

        model.Series.Add(heatMapSeries);
        WriteToSVG(model, filePath);
    }

    /// <summary>
    /// Method that takes the gaze point data and plots them as a scatterplot
    /// </summary>
    /// <param name="filePath">File path where the scatter plot will be created + the name of the file at the end of the path</param>
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
    }

    /// <summary>
    /// Method that creates the test object plot with movement lines and where the object was seen
    /// </summary>
    /// <param name="saveFilePath">File path where the plot will be created + the name of the file at the end of the path</param>
    /// <param name="graphFilePath">File path where the save file of test object is located</param>
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
            Minimum = double.Parse(minCoords[0]),
            Maximum = double.Parse(maxCoords[0]),
            Key = "Horizontal",
            Title = "\nLine = Movement to an object\n\n" +
                    "Green orb = Positions where a full object has been seen\n\n" +
                    "Ligth blue orb = Positions where a rigth side object has been seen\n\n" +
                    "Dark blue orb = Positions where a left side object has been seen\n\n" +
                    "Ligth orange = A rigth side object that has not been seen\n\n" +
                    "Dark orange = A left side object that has not been seen\n\n" +
                    "Red = A full object that has not been seen"
        });
        model.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Minimum = double.Parse(minCoords[1]), Maximum = double.Parse(maxCoords[1]), Key = "Vertical" });
        scatterSeries.XAxisKey = "Horizontal";
        scatterSeries.YAxisKey = "Vertical";
        //adds objects to the graph
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

    /// <summary>
    /// Method that creates plot file and saves it at given location
    /// </summary>
    /// <param name="model">The plot to be saved</param>
    /// <param name="newFilePath">Path where the plot will be created + the name of the plot</param>
    public void WriteToSVG(PlotModel model, string newFilePath)
    {
        using (var stream = File.Create(newFilePath + ".svg"))
        {
            var exporter = new SvgExporter { Width = 1920, Height = 1380 };
            exporter.Export(model, stream);
        }
    }
    
}
