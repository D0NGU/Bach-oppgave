using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tobii.XR;
using System.IO;
using System;

public class TestStarter : MonoBehaviour
{
    private Coroutine countdownCoroutine;

    public int time = 3;

    [SerializeField]
    private TMP_Text countdownText;
    [SerializeField]
    private TMP_Text startButtonText;
    [SerializeField]
    private Button loadTestButton;
    [SerializeField]
    private GameObject testObjectParent;
    [SerializeField]
    private GameObject mainView;
    [SerializeField]
    private GameObject scrollView;
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
        if (countdownCoroutine != null && !TestDataStatic.testIsRunning)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
            countdownText.gameObject.SetActive(false);
            testObjectParent.SetActive(true);
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
            EnableTestLoading(true);
        }
        // If the test is not running and the coroutine has not started yet
        else if(!TestDataStatic.testIsRunning)
        {
            inputField.text = "";
            testObjectParent.SetActive(false);
            countdownCoroutine = StartCoroutine(CountDown());
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Stop Test";
            EnableTestLoading(false);
        }
        // If the countdown coroutine is finished and the test in running/in progress
        else
        {
            countdownCoroutine = null;
            StartStopTest();
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";

            saveFileView.SetActive(true);
            mainView.SetActive(false);
            EnableTestLoading(true);
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

        StartStopTest();
    }

    public void StartStopTest()
    {

        TestDataStatic.testIsRunning = !TestDataStatic.testIsRunning;

        

        if (TestDataStatic.testIsRunning)
        {
            testResultsSaver.StartWritingGazeDotsData();
        }
        else
        {
            testResultsSaver.CloseStreamWriter();
            loadTestButton.interactable = true;
        }

        countdownCoroutine = null;
        foreach (Transform child in testObjectParent.transform)
        {
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            so.StartTest();
        }
    }

    public void CheckIfFileExists()
    {
        if (Directory.Exists(TestDataStatic.testResultFolder + inputField.text))
        {
            overwriteSaveConfirmationView.SetActive(true);
            saveFileView.SetActive(false);
        }
        else
        {
            mainView.SetActive(true);
            saveFileView.SetActive(false);

            Directory.CreateDirectory(TestDataStatic.testResultFolder + inputField.text);

            testResultsSaver.SaveGazeData(inputField.text);
            testResultsSaver.SaveTestResults(inputField.text);
        }
    }

    public void OverwriteFile()
    {
        testResultsSaver.SaveGazeData(inputField.text);
        testResultsSaver.SaveTestResults(inputField.text);
    }

    private void EnableTestLoading(bool enable)
    {
        loadTestButton.interactable = enable;
        scrollView.SetActive(false);
    }
}
