using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using System.IO;
using System;

public class TestResultsSaver : MonoBehaviour
{
    [SerializeField]
    private GameObject testObjectParent;

    private SaveResultsScript saveToFile = new();

    // The path of the file where the data was initially stored during runtime
    private string temporaryDataFilePath = Path.Combine(Environment.CurrentDirectory, "Assets/TestData/TemporaryDataFile/data.csv");

    private List<Vector3> eyeGazeData = new();
    private StreamWriter sw;
    private Plotting plotting;
    private bool writeGazeData = false;

    private void Awake()
    {
        plotting = GetComponent<Plotting>();

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

    public void SaveGazeData(string fileName)
    {
        Debug.Log("Data saved.");

        writeGazeData = false;

        // The path of the file to store the data specified by the user
        string newFilePath = Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + fileName + "/" + fileName + "_gazePoints" + ".csv");

        if (File.Exists(newFilePath))
        {
            File.Delete(newFilePath);
        }

        plotting.ReadAndPlot(temporaryDataFilePath);
        plotting.CreateHeatmap(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + fileName + "/" + fileName + "_heatmap"));
        plotting.CreateScatterPlot(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + fileName + "/" + fileName + "_scatterplot"));

        // Moves data from the temporary data file to a new file with user specified name
        File.Move(temporaryDataFilePath, newFilePath);
    }

    public void WriteAreaBoundsToCSV()
    {
        sw = new StreamWriter(temporaryDataFilePath);

        TestArea testArea = GameObject.Find("TestArea").GetComponent<TestArea>();
        sw.WriteLine(testArea.bottomLeftBackCorner.transform.position.ToString());
        sw.WriteLine(testArea.topRightBackCorner.transform.position.ToString());
    }

    public void CloseStreamWriter()
    {
        writeGazeData = false;

        sw.Close();
    }

    public void ClearGazeData()
    {
        eyeGazeData.Clear();
    }


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

        saveToFile.SaveToJSON(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + fileName + "/" + fileName + "_objects"), resultsData);
        saveToFile = new();
        
        plotting.CreateTestObjectPlot(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + fileName + "/" + fileName + "_testObjectPlot"), Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + fileName + "/" + fileName + "_objects"));
    }

    public void StartWritingGazeDotsData()
    {
        WriteAreaBoundsToCSV();
        ClearGazeData();

        writeGazeData = true;

    }


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

    public void SaveRandomizedTestResults(string fileName)
    {
        saveToFile.SaveRandomizedToJSON(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + fileName + "/" + fileName + "_objects"));
        saveToFile = new();

    }

    public void ClearRandomizedData()
    {
        saveToFile.ClearRandomizedResultsList();
    }

    public void WriteWaveNumber(int waveNumber)
    {
        sw.WriteLine("Wavenumber " + waveNumber);
    }
}
