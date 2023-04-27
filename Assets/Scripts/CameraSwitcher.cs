using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using UnityEngine.SpatialTracking;

public class CameraSwitcher : MonoBehaviour
{

    [SerializeField]
    private Camera displayCamera;
    [SerializeField]
    private Camera staticCamera;
    [SerializeField]
    private Camera trackedCamera;
    [SerializeField]
    private TMP_Text changePlayerCameraText;
    [SerializeField]
    private TMP_Text toggleDisplayCameraText;
    [SerializeField]
    private TrackedPoseDriver tpd;

    public void ChangeCameraView()
    {
        if (staticCamera.stereoTargetEye == StereoTargetEyeMask.None)
        {
            staticCamera.stereoTargetEye = StereoTargetEyeMask.Both;
            staticCamera.depth = 0;
            trackedCamera.stereoTargetEye = StereoTargetEyeMask.None;
            trackedCamera.depth = -1;
            

            changePlayerCameraText.GetComponent<TextMeshProUGUI>().text = "Change to tracked cam";
        }
        else
        {
            staticCamera.stereoTargetEye = StereoTargetEyeMask.None;
            staticCamera.depth = -1;
            trackedCamera.stereoTargetEye = StereoTargetEyeMask.Both;
            trackedCamera.depth = 0;

            changePlayerCameraText.GetComponent<TextMeshProUGUI>().text = "Change to static cam";
        }
    }

    
    public void ToggleDisplayCamera()
    {
        displayCamera.gameObject.SetActive(!displayCamera.gameObject.activeSelf);
        if (displayCamera.gameObject.activeSelf)
        {
            toggleDisplayCameraText.GetComponent<TextMeshProUGUI>().text = "Show player perspective";
        }
        else
        {
            toggleDisplayCameraText.GetComponent<TextMeshProUGUI>().text = "Return to default view";
        }
    }
}
