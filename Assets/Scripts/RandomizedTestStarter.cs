using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

/// <summary>
/// Responsible for starting and controlling a randomized test.
/// </summary>
public class RandomizedTestStarter : MonoBehaviour
{
    private TestArea testArea;
    private Coroutine testCoroutine;
    private Coroutine countdownCoroutine;

    [SerializeField]
    [Tooltip("Prefab of selectable object")]
    private GameObject spherePrefab;
    [SerializeField]
    [Tooltip("Parent object of all selectable objects in the editor test area")]
    private GameObject testObjectParent;
    [SerializeField]
    [Tooltip("UI slider for changing the percentage of test object that are full spheres")]
    private Slider sliderPercentageFullSpheres;
    [SerializeField]
    [Tooltip("UI slider for changing the percentage of test object that are left halves")]
    private Slider sliderPercentageLeftHalves;
    [SerializeField]
    [Tooltip("TMPro text of the start button")]
    private TMP_Text startButtonText;
    [SerializeField]
    [Tooltip("Game object containing the main UI options for the randomized test")]
    private GameObject mainView;
    [SerializeField]
    [Tooltip("Game object containing the UI options for saving the test results")]
    private GameObject saveFileView;
    [SerializeField]
    [Tooltip("TMPro input field for the test results folder name")]
    private TMP_InputField inputField;
    [SerializeField]
    [Tooltip("Game object containing the confirmation view for overwriting an existing file")]
    private GameObject overwriteSaveConfirmationView;
    [SerializeField]
    [Tooltip("TestResultSaver script")]
    private TestResultsSaver testResultsSaver;
    [SerializeField]
    [Tooltip("TMPro text showing the test start countdown")]
    private TMP_Text countdownText;

    private bool showLeft = true;
    private bool showRight = true;

    // Randomized test parameters
    private int percentageFullSpheres = 100;
    private int percentageLeftHalves = 0;
    private string displayedSide = "both";
    private int time = 3;
    private int spawnInterval = 10;
    private int spawnAmount = 1;

    private int waveNumber = 0;

    /// <summary>
    /// Object used to store all the parameters of the randomized test.
    /// </summary>
    private RandomizedTestParametersClass randomizedTestData;


    private void Awake()
    {
        TestDataStatic.visionDetectionTime = 1.5f;

        testArea = GameObject.Find("TestArea").GetComponent<TestArea>();
    }

    /// <summary>
    /// Handles starting and stopping the randomized test
    /// </summary>
    public void StartCountdown()
    {
        // If the coroutine is not null, meaning the countdown is in progress, the countdown is stopped
        if (countdownCoroutine != null && !TestDataStatic.testIsRunning)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
            countdownText.gameObject.SetActive(false);

            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
        }
        // If the test is not running and the coroutine has not started yet, the countdown is started
        else if (!TestDataStatic.testIsRunning)
        {
            inputField.text = "";

            countdownCoroutine = StartCoroutine(CountDown());

            startButtonText.GetComponent<TextMeshProUGUI>().text = "Stop Test";
        }
        // If the countdown coroutine is finished and the test in running/in progress, the test is stopped
        else
        {
            countdownCoroutine = null;

            //Saves data when the test is stopped in order to not miss the current state
            testResultsSaver.SaveRandomizedWave(randomizedTestData);

            StartStopTest();

            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
        }
    }

    /// <summary>
    /// Handles the countdown run before the randomized test starts.
    /// Has to be run as a coroutine. 
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator CountDown()
    {
        int localTime = time;

        countdownText.gameObject.SetActive(true);
        countdownText.GetComponent<TextMeshProUGUI>().text = localTime.ToString();
        while (localTime > 0)
        {
            yield return new WaitForSeconds(1);
            localTime--;
            countdownText.GetComponent<TextMeshProUGUI>().text = localTime.ToString();
        }
        countdownText.gameObject.SetActive(false);

        StartStopTest();
    }

    /// <summary>
    /// Handles preparation for the randomized test and starts/stops the test coroutine.
    /// </summary>
    public void StartStopTest()
    {
        waveNumber = 0;

        TestDataStatic.testIsRunning = !TestDataStatic.testIsRunning;

        if (TestDataStatic.testIsRunning) testResultsSaver.StartWritingGazeDotsData();
        else testResultsSaver.CloseStreamWriter();

        // If the test coroutine is not null, the coroutine is stopped
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

    /// <summary>
    /// Instantiated a wave of randomly positioned selectable objects according to the test parameters.
    /// Has to be manually stopped.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator RandomizedSpawn()
    {
        while (true) 
        {

            ClearTestArea();

            var dict = testArea.GetTestAreaBounds();
            int numberOfFullSpheres = (spawnAmount * percentageFullSpheres) / 100;
            int numberOfLeftHalves = ((spawnAmount - numberOfFullSpheres) * percentageLeftHalves) / 100;

            testResultsSaver.WriteWaveNumber(waveNumber);

            // Instantiates a number of object equal to the spawnAmount
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
            randomizedTestData = GetRandomizedTestParameters();

            yield return new WaitForSeconds(spawnInterval);

            testResultsSaver.SaveRandomizedWave(randomizedTestData);
            waveNumber++;
        }
    }

    /// <summary>
    /// Deletes all object in the test area
    /// </summary>
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

    /// <summary>
    /// Checks if a folder with the chosen name already exists.
    /// Shows option to overwrite if it does, saves the data if not. 
    /// </summary>
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
            testResultsSaver.SaveRandomizedTestResults(inputField.text);
        }
    }

    /// <summary>
    /// Saves the test result data
    /// </summary>
    public void OverwriteFile()
    {
        testResultsSaver.SaveGazeData(inputField.text);
        testResultsSaver.SaveRandomizedTestResults(inputField.text);
    }

    /// <summary>
    /// Retrieves all the parameters of the randomized test.
    /// </summary>
    /// <returns>Object containing all the parameters of the randomized test.</returns>
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
