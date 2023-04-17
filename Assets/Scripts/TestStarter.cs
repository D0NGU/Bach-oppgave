using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Tobii.XR;
using System.IO;
using System;

public class TestStarter : MonoBehaviour
{
    private Coroutine coroutine;
    private bool testIsRunning = false;
    private List<Vector3> eyeGazeData = new();
    private StreamWriter sw;
    private Plotting plotting;

    // The path of the file where the data was initially stored during runtime
    private string temporaryDataFilePath = Path.Combine(Environment.CurrentDirectory, "Assets/TestData/TemporaryDataFile/data.csv");

    public int time = 3;
    public TMP_Text countdownText;
    public TMP_Text startButtonText;
    public GameObject testObjectParent;
    public GameObject mainView;
    public GameObject saveFileView;
    public TMP_InputField inputField;
    public GameObject overwriteSaveConfirmationView;


    private void Awake()
    {
        plotting = GetComponent<Plotting>();
    }

    private void Update()
    {
        if (testIsRunning)
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

    public void StartCountdown()
    {
        // If the coroutine is not null, meaning the countdown is in progress
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            countdownText.gameObject.SetActive(false);
            testObjectParent.SetActive(true);
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
        }
        // If the test is not running and the coroutine has not started yet
        else if(!testIsRunning)
        {
            inputField.text = "";
            testObjectParent.SetActive(false);
            coroutine = StartCoroutine(CountDown());
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Stop Test";
        }
        // If the countdown coroutine is finished and the test in running/in progress
        else 
        {
            sw.Close();

            StartTest();
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";

            saveFileView.SetActive(true);
            mainView.SetActive(false);
        }
    }

    public IEnumerator CountDown()
    {
        int localTime = time;
        countdownText.GetComponent<TextMeshProUGUI>().text = localTime.ToString();
        countdownText.gameObject.SetActive(true);

        while (localTime > 0)
        {
            Debug.Log(localTime);
            yield return new WaitForSeconds(1);
            localTime--;
            countdownText.GetComponent<TextMeshProUGUI>().text = localTime.ToString();
        }
        countdownText.gameObject.SetActive(false);
        testObjectParent.SetActive(true);

        sw = new StreamWriter(temporaryDataFilePath);
        WriteAreaBoundsToCSV(sw);
        StartTest();
    }

    public void StartTest()
    {
        eyeGazeData.Clear();
        testIsRunning = !testIsRunning;
        coroutine = null;
        foreach (Transform child in testObjectParent.transform)
        {
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            so.StartTest();
        }
    }

    public void CheckIfFileExists()
    {
        if (File.Exists(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + inputField.text + ".csv")))
        {
            overwriteSaveConfirmationView.SetActive(true);
            saveFileView.SetActive(false);
        }
        else
        {
            mainView.SetActive(true);
            saveFileView.SetActive(false);
            SaveGazeData();
        }
    }

    public void SaveGazeData()
    {
        Debug.Log("Data saved.");

        // The path of the file to store the data specified by the user
        string newFilePath = Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + inputField.text + ".csv");

        if (File.Exists(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + inputField.text + ".csv")))
        {
            File.Delete(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + inputField.text + ".csv"));
        }

        plotting.ReadAndPlot(temporaryDataFilePath);
        plotting.CreateHeatmap(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + inputField.text + "_heatmap"));
        plotting.CreateScatterPlot(Path.Combine(Environment.CurrentDirectory, "Assets/TestData/" + inputField.text + "_scatterplot"));

        // Moves data from the temporary data file to a new file with user specified name
        File.Move(temporaryDataFilePath, newFilePath);
    }

    public void WriteAreaBoundsToCSV(StreamWriter sw)
    {
        TestArea testArea = GameObject.Find("TestArea").GetComponent<TestArea>();
        sw.WriteLine(testArea.bottomLeftBackCorner.transform.position.ToString());
        sw.WriteLine(testArea.topRightBackCorner.transform.position.ToString());
    }
}
