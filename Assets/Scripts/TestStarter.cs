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

    public int time = 3;

    [SerializeField]
    private TMP_Text countdownText;
    [SerializeField]
    private TMP_Text startButtonText;
    [SerializeField]
    private GameObject testObjectParent;
    [SerializeField]
    private GameObject mainView;
    [SerializeField]
    private GameObject saveFileView;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private GameObject overwriteSaveConfirmationView;
    [SerializeField]
    private TestResultsSaver testResultsSaver;


    public void StartCountdown()
    {
        // If the coroutine is not null, meaning the countdown is in progress
        if (coroutine != null && !TestDataStatic.testIsRunning)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            countdownText.gameObject.SetActive(false);
            testObjectParent.SetActive(true);
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
        }
        // If the test is not running and the coroutine has not started yet
        else if(!TestDataStatic.testIsRunning)
        {
            inputField.text = "";
            testObjectParent.SetActive(false);
            coroutine = StartCoroutine(CountDown());
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Stop Test";
        }
        // If the countdown coroutine is finished and the test in running/in progress
        else
        {
            coroutine = null;
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

        StartTest();
    }

    public void StartTest()
    {

        TestDataStatic.testIsRunning = !TestDataStatic.testIsRunning;

        if (TestDataStatic.testIsRunning) testResultsSaver.StartWritingGazeDotsData();
        else testResultsSaver.CloseStreamWriter();

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

            testResultsSaver.SaveGazeData(inputField.text);
            testResultsSaver.SaveTestResults(inputField.text);
        }
    }

    public void OverwriteFile()
    {
        testResultsSaver.SaveGazeData(inputField.text);
        testResultsSaver.SaveTestResults(inputField.text);
    }
}
