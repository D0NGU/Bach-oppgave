using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private Camera staticCamera;
    [SerializeField]
    private Camera trackedCamera;
    [SerializeField]
    private TMP_Text textDisplay;

    public void ChangeCameraView()
    {
        if (staticCamera.stereoTargetEye == StereoTargetEyeMask.None)
        {
            staticCamera.stereoTargetEye = StereoTargetEyeMask.Both;
            staticCamera.depth = 0;
            trackedCamera.stereoTargetEye = StereoTargetEyeMask.None;
            trackedCamera.depth = -1;


            textDisplay.GetComponent<TextMeshProUGUI>().text = "Change to tracked cam";
        }
        else
        {
            staticCamera.stereoTargetEye = StereoTargetEyeMask.None;
            staticCamera.depth = -1;
            trackedCamera.stereoTargetEye = StereoTargetEyeMask.Both;
            trackedCamera.depth = 0;


            textDisplay.GetComponent<TextMeshProUGUI>().text = "Change to static cam";
        }
    }
}
