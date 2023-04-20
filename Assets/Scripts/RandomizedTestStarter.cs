using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class RandomizedTestStarter : MonoBehaviour
{
    private TestArea testArea;
    private Coroutine testCoroutine;
    private Coroutine countdownCoroutine;

    public int time = 3;

    [SerializeField]
    private GameObject spherePrefab;
    [SerializeField]
    private GameObject testObjectParent;
    [SerializeField]
    private Slider sliderPercentageFullSpheres;
    [SerializeField]
    private Slider sliderPercentageLeftHalves;
    [SerializeField]
    private TMP_Text startButtonText;
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
    [SerializeField]
    private TMP_Text countdownText;

    private bool showLeft = true;
    private bool showRight = true;
    private int percentageFullSpheres = 100;
    private int percentageLeftHalves = 0;
    private string displayedSide = "both";

    public int spawnInterval = 10;
    public int spawnAmount = 1;

    private void Awake()
    {
        TestDataStatic.visionDetectionTime = 1.5f;

        testArea = GameObject.Find("TestArea").GetComponent<TestArea>();
    }

    public void StartCountdown()
    {
        // If the coroutine is not null, meaning the countdown is in progress
        if (countdownCoroutine != null && !TestDataStatic.testIsRunning)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
            countdownText.gameObject.SetActive(false);

            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
        }
        // If the test is not running and the coroutine has not started yet
        else if (!TestDataStatic.testIsRunning)
        {
            inputField.text = "";

            countdownCoroutine = StartCoroutine(CountDown());

            startButtonText.GetComponent<TextMeshProUGUI>().text = "Stop Test";
        }
        // If the countdown coroutine is finished and the test in running/in progress
        else
        {
            countdownCoroutine = null;

            //Saves data when the test is stopped in order to not miss the current state
            testResultsSaver.SaveRandomizedWave(GetRandomizedTestParameters());

            StartTest();

            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
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

        StartTest();
    }


    public void StartTest()
    {
        TestDataStatic.testIsRunning = !TestDataStatic.testIsRunning;

        if (TestDataStatic.testIsRunning) testResultsSaver.StartWritingGazeDotsData();
        else testResultsSaver.CloseStreamWriter();

        if (testCoroutine != null)
        {
            StopCoroutine(testCoroutine);
            testCoroutine = null;
            ClearTestArea();

            saveFileView.SetActive(true);
            mainView.SetActive(false);

            return;
        }

        testCoroutine = StartCoroutine(RandomizedSpawn());
    }

    private IEnumerator RandomizedSpawn()
    {
        while (true) 
        {

            ClearTestArea();

            var dict = testArea.GetTestAreaBounds();
            int numberOfFullSpheres = (spawnAmount * percentageFullSpheres) / 100;
            int numberOfLeftHalves = ((spawnAmount - numberOfFullSpheres) * percentageLeftHalves) / 100;

            for (int i = 0; i < spawnAmount; i++)
            {
                GameObject gameObject = Instantiate(spherePrefab, testObjectParent.transform);


                if (i >= numberOfFullSpheres && i < (numberOfFullSpheres + numberOfLeftHalves)) 
                {
                    gameObject.transform.Find("Sphere").gameObject.GetComponent<SelectableObject>().ChangeToLeftHalf();
                }
                else if (i >= (numberOfFullSpheres + numberOfLeftHalves)) 
                {
                    gameObject.transform.Find("Sphere").gameObject.GetComponent<SelectableObject>().ChangeToRightHalf();
                }

                
                if (showLeft && !showRight)
                {
                    gameObject.transform.position = new Vector3(UnityEngine.Random.Range(dict["minX"], 0.0f), UnityEngine.Random.Range(dict["minY"], dict["maxY"]), UnityEngine.Random.Range(dict["minZ"], dict["maxZ"]));
                    displayedSide = "left";
                }
                else if (!showLeft && showRight)
                {
                    gameObject.transform.position = new Vector3(UnityEngine.Random.Range(0.0f, dict["maxX"]), UnityEngine.Random.Range(dict["minY"], dict["maxY"]), UnityEngine.Random.Range(dict["minZ"], dict["maxZ"]));
                    displayedSide = "rigth";
                }
                else
                {
                    gameObject.transform.position = new Vector3(UnityEngine.Random.Range(dict["minX"], dict["maxX"]), UnityEngine.Random.Range(dict["minY"], dict["maxY"]), UnityEngine.Random.Range(dict["minZ"], dict["maxZ"]));
                    displayedSide = "both";
                }

                gameObject.transform.Find("Sphere").gameObject.GetComponent<SelectableObject>().StartTest();
            }

            // Stores all relevant test parameters 
            RandomizedTestParametersClass randomizedTestData = GetRandomizedTestParameters();

            yield return new WaitForSeconds(spawnInterval);

            testResultsSaver.SaveRandomizedWave(randomizedTestData);
        }
    }

    private void ClearTestArea()
    {
        foreach (Transform child in testObjectParent.transform)
        {
            Destroy(child.gameObject);
        }
    }


    public void ShowLeftSide()
    {
        showLeft = true;
        showRight = false;
    }

    public void ShowRightSide()
    {
        showLeft = false;
        showRight = true;
    }

    public void ShowBothSides()
    {
        showLeft = true;
        showRight = true;
    }

    public void SetFullSpherePercentage()
    {
        percentageFullSpheres = (int) sliderPercentageFullSpheres.value;
    }

    public void SetLeftHalvesPercentage()
    {
        percentageLeftHalves = (int) sliderPercentageLeftHalves.value;
    }

    public void SetSpawnAmount(string text)
    {
        if (int.TryParse(text, out int result))
        {
            spawnAmount = result;
        }
    }

    public void SetSpawnInterval(string text)
    {
        if (int.TryParse(text, out int result))
        {
            spawnInterval = result;
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
            testResultsSaver.SaveRandomizedTestResults(inputField.text);
        }
    }

    public void OverwriteFile()
    {
        testResultsSaver.SaveGazeData(inputField.text);
        testResultsSaver.SaveRandomizedTestResults(inputField.text);
    }

    public RandomizedTestParametersClass GetRandomizedTestParameters()
    {
        RandomizedTestParametersClass randomizedTestData = new();
        randomizedTestData.amount = spawnAmount;
        randomizedTestData.interval = spawnInterval;
        randomizedTestData.percentFullSpheres = percentageFullSpheres;
        randomizedTestData.percentLeftSpheres = percentageLeftHalves;
        randomizedTestData.displayedSide = displayedSide;

        return randomizedTestData;
    }
}
