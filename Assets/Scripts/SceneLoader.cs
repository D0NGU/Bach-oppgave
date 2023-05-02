using UnityEngine;
using ViveSR.anipal.Eye;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for scene navigation
/// </summary>
public class SceneLoader : MonoBehaviour
{

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadRunTest()
    {
        SceneManager.LoadScene("RunTest");
    }

    public void ExitApplication()
    {
        Debug.Log("Exiting application");
        Application.Quit();
    }

    public void LoadEditor()
    {
        SceneManager.LoadScene("EditTest");
    }

    public void LoadRandomizedTest()
    {
        SceneManager.LoadScene("RandomizedTest");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainScreen");
    }

    public void LoadCalibrator()
    {
        IntPtr test = IntPtr.Zero;
        SRanipal_Eye_API.LaunchEyeCalibration(test);
    }

    /// <summary>
    /// Toggle a Game Object to active or inactive.
    /// </summary>
    /// <param name="gameObject"></param>
    public void ToggleActive(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    /// <summary>
    /// Opens a file explorer at the path where the test files are saved.
    /// </summary>
    public void OpenTestFolder()
    {
        string path = TestDataStatic.testFolderPath.Replace("/", "\\");
        System.Diagnostics.Process.Start("explorer.exe", @path);
    }

}
