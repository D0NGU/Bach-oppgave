using UnityEngine;
using TMPro;

/// <summary>
/// Handles all changes of which camera is displayed where. 
/// </summary>
public class CameraSwitcher : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The camera to toggle on and off.")]
    private Camera displayCamera;
    [SerializeField]
    [Tooltip("TMPro Text Element of the button used to toggle the display camera.")]
    private TMP_Text toggleDisplayCameraText;


    /// <summary>
    /// Toggles the display camera on and off. When on, the XR Camera is displayed on the main display. 
    /// When off, a separate camera view is displayed. 
    /// </summary>
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
