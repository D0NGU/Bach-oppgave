using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using System.IO;

/// <summary>
/// Responsible for saving the test results of a normal and
/// a randomized test.
/// </summary>
public class TestResultsSaver : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The parent object of the selectable objects in the test area")]
    private GameObject testObjectParent;

    private SaveResultsScript saveToFile;

    /// <summary>
    /// The path of the file where the data was initially stored during runtime
    /// </summary>
    private string temporaryDataFilePath;

    private List<Vector3> eyeGazeData = new();
    private StreamWriter sw;
    private Plotting plotting;
    private bool writeGazeData = false;

    private void Awake()
    {
        plotting = GetComponent<Plotting>();
        saveToFile = new();
        temporaryDataFilePath = TestDataStatic.testResultFolder + ".TemporaryDataFile/data.csv";

        TestDataStatic.testIsRunning = false;
    }

    private void Update()
    {
        // Writes eye gaze data to file if a test is running
        if (TestDataStatic.testIsRunning && writeGazeData)
        {
            var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
            if (eyeTrackingData.GazeRay.IsValid)
            {
                var rayOrigin = eyeTrackingData.GazeRay.Origin;
                var rayDirection = eyeTrackingData.GazeRay.Direction;
                RaycastHit hit;
                Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity);
                sw.WriteLine(hit.point.ToString());
                eyeGazeData.Add(hit.point);
            }
        }
    }

    /// <summary>
    /// Saves the gaze data of a test to CSV file.
    /// Creates the relevant plots using the data.
    /// </summary>
    /// <param name="fileName">Name of the file to save the gaze data to</param>
    public void SaveGazeData(string fileName)
    {
        Debug.Log("Data saved.");

        writeGazeData = false;

        // The path of the file to store the data specified by the user
        string newFilePath = TestDataStatic.testResultFolder + fileName + "/" + fileName + "_gazePoints" + ".csv";

        if (File.Exists(newFilePath))
        {
            File.Delete(newFilePath);
        }

        plotting.ReadAndPlot(temporaryDataFilePath);
        plotting.CreateHeatmap(TestDataStatic.testResultFolder + fileName + "/" + fileName + "_heatmap");
        plotting.CreateScatterPlot(TestDataStatic.testResultFolder + fileName + "/" + fileName + "_scatterplot");

        // Moves data from the temporary data file to a new file with user specified name
        File.Move(temporaryDataFilePath, newFilePath);
    }

    /// <summary>
    /// Writes the test area bounds (min and max x, y, z coordinates) 
    /// to the gaze data file.
    /// </summary>
    public void WriteAreaBoundsToCSV()
    {
        sw = new StreamWriter(temporaryDataFilePath);

        TestArea testArea = GameObject.Find("TestArea").GetComponent<TestArea>();
        sw.WriteLine(testArea.bottomLeftBackCorner.transform.position.ToString());
        sw.WriteLine(testArea.topRightBackCorner.transform.position.ToString());
    }

    /// <summary>
    /// Stops writing of the gaze data and closes the StreamWriter
    /// </summary>
    public void CloseStreamWriter()
    {
        writeGazeData = false;

        sw.Close();
    }

    /// <summary>
    /// Clears the list containing the gaze data points
    /// </summary>
    public void ClearGazeData()
    {
        eyeGazeData.Clear();
    }

    /// <summary>
    /// Saves the selecable objects of the test to json file.
    /// Creates a plot of these objects.
    /// </summary>
    /// <param name="fileName">Name of the file to save the test results to</param>
    public void SaveTestResults(string fileName)
    {
        ResultsDataListClass resultsData = new();

        foreach (Transform child in testObjectParent.transform)
        {
            SelectableObjectResultsDataClass dataClass = new();
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            dataClass.startPosistion = so.startPos;
            dataClass.endPosistion = so.endPos;
            dataClass.positionWhenSeen = so.positionWhenSeen;
            dataClass.scale = so.scale;
            dataClass.moveTime = so.moveTime;
            dataClass.startDelay = so.startDelay;
            dataClass.hasMovement = so.hasMovement;
            dataClass.loopMovement = so.loopMovement;
            dataClass.objectType = so.objectType;
            dataClass.hasBeenSeen = so.hasBeenSeen;
            dataClass.timePassedBeforeSeen = so.timePassedBeforeSeen;

            resultsData.AddObjectDataToList(dataClass);
        }

        resultsData.visionDetectionTime = TestDataStatic.visionDetectionTime;
        resultsData.playerDistance = TestDataStatic.playerDistance;

        saveToFile.SaveToJSON(TestDataStatic.testResultFolder + fileName + "/" + fileName + "_objects", resultsData);
        saveToFile = new();
        
        plotting.CreateTestObjectPlot(TestDataStatic.testResultFolder + fileName + "/" + fileName + "_testObjectPlot", TestDataStatic.testResultFolder + fileName + "/" + fileName + "_objects");
    }

    /// <summary>
    /// Starts the writing of gaze data to file
    /// </summary>
    public void StartWritingGazeDotsData()
    {
        WriteAreaBoundsToCSV();
        ClearGazeData();

        writeGazeData = true;

    }

    /// <summary>
    /// Saves the selecable objects of a wave/set of a randomized test to json file.
    /// </summary>
    /// <param name="randomizedTestData">Class containing the parameters for the wave/set</param>
    public void SaveRandomizedWave(RandomizedTestParametersClass randomizedTestData)
    {
        RandomizedResultsDataClass resultsData = new();

        foreach (Transform child in testObjectParent.transform)
        {
            SelectableObjectResultsDataClass dataClass = new();
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            dataClass.startPosistion = so.startPos;
            dataClass.endPosistion = so.endPos;
            dataClass.positionWhenSeen = so.positionWhenSeen;
            dataClass.scale = so.scale;
            dataClass.moveTime = so.moveTime;
            dataClass.startDelay = so.startDelay;
            dataClass.hasMovement = so.hasMovement;
            dataClass.loopMovement = so.loopMovement;
            dataClass.objectType = so.objectType;

            dataClass.hasBeenSeen = so.hasBeenSeen;
            dataClass.timePassedBeforeSeen = so.timePassedBeforeSeen;

            resultsData.AddObjectDataToList(dataClass);
        }

        resultsData.randomizedTestData = randomizedTestData;

        resultsData.visionDetectionTime = TestDataStatic.visionDetectionTime;
        resultsData.playerDistance = TestDataStatic.playerDistance;

        saveToFile.AddToRandomizedResultsList(resultsData);

    }

    /// <summary>
    /// Saves all waves/sets of a randomized test to json file
    /// </summary>
    /// <param name="fileName">Name of the file to save the test results to</param>
    public void SaveRandomizedTestResults(string fileName)
    {
        saveToFile.SaveRandomizedToJSON(TestDataStatic.testResultFolder + fileName + "/" + fileName + "_objects");
        saveToFile = new();

    }

    /// <summary>
    /// Clears the list containing all the results data of the randomized waves/sets
    /// </summary>
    public void ClearRandomizedData()
    {
        saveToFile.ClearRandomizedResultsList();
    }

    /// <summary>
    /// Writes the wave number of a randomized wave/set to the gaze data file
    /// </summary>
    /// <param name="waveNumber">Wave number</param>
    public void WriteWaveNumber(int waveNumber)
    {
        sw.WriteLine("Wavenumber " + waveNumber);
    }
}
