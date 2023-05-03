using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

/// <summary>
/// Responsible for starting and controlling a normal test.
/// </summary>
public class TestStarter : MonoBehaviour
{
    private Coroutine countdownCoroutine;

    public int time = 3;

    [SerializeField]
    [Tooltip("TMPro text showing the test start countdown")]
    private TMP_Text countdownText;
    [SerializeField]
    [Tooltip("TMPro text of the start button")]
    private TMP_Text startButtonText;
    [SerializeField]
    [Tooltip("UI Button for loading a test")]
    private Button loadTestButton;
    [SerializeField]
    [Tooltip("Parent object of all selectable objects in the editor test area")]
    private GameObject testObjectParent;
    [SerializeField]
    [Tooltip("Game object containing the main UI options for the randomized test")]
    private GameObject mainView;
    [SerializeField]
    private GameObject scrollView;
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

    /// <summary>
    /// Handles starting and stopping the test
    /// </summary>
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

    /// <summary>
    /// Handles the countdown run before the test starts.
    /// Has to be run as a coroutine. 
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator CountDown()
    {
        int localTime = time;
        countdownText.GetComponent<TextMeshProUGUI>().text = localTime.ToString();
        countdownText.gameObject.SetActive(true);

        while (localTime > 0)
        {
            yield return new WaitForSeconds(1);
            localTime--;
            countdownText.GetComponent<TextMeshProUGUI>().text = localTime.ToString();
        }
        countdownText.gameObject.SetActive(false);
        testObjectParent.SetActive(true);

        StartStopTest();
    }

    /// <summary>
    ///  Handles preparation for the test and starts/stops the test coroutine.
    /// </summary>
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

        // Start the test/movement for all selectable objects in the test area
        foreach (Transform child in testObjectParent.transform)
        {
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            so.StartTest();
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
            testResultsSaver.SaveTestResults(inputField.text);
        }
    }

    /// <summary>
    /// Saves the test result data
    /// </summary>
    public void OverwriteFile()
    {
        testResultsSaver.SaveGazeData(inputField.text);
        testResultsSaver.SaveTestResults(inputField.text);
    }

    /// <summary>
    /// Enables/disables the test loading button
    /// to prevent a test from being loaded while a test is running.
    /// </summary>
    /// <param name="enable">true (enable) or false (disable)</param>
    private void EnableTestLoading(bool enable)
    {
        loadTestButton.interactable = enable;
        scrollView.SetActive(false);
    }
}
