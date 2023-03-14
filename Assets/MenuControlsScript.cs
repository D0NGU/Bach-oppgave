using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;
using System;

public class MenuControlsScript : MonoBehaviour
{

    public void exitApplication()
    {
        Debug.Log("Exiting application");
        Application.Quit();
    }

    public void enterEditor()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void enterCalibrator()
    {
        IntPtr test = IntPtr.Zero;
        SRanipal_Eye_API.LaunchEyeCalibration(test);
    }


}
