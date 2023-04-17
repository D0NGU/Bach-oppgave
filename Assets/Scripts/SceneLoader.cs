using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;
using System;
using UnityEngine.SceneManagement;

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


}
