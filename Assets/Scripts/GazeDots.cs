using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Tobii.XR;

/// <summary>
/// Responsible for instantiating an object at the collision point
/// of the players gaze ray and game object with valid colliders
/// to visualize their gaze.
/// </summary>
public class GazeDots : MonoBehaviour
{
    [SerializeField] 
    private InputActionAsset ActionAsset;
    [SerializeField]
    [Tooltip("UI Button used to toggle Gaze Dots on and off.")]
    private Button button;
    [SerializeField]
    [Tooltip("Game Object prefab used to visualize the gaze.")]
    private GameObject gazeSphere;

    private GameObject sphere;

    [Tooltip("Controls whether to enable gaze dots.")]
    public bool showGazeDots = false;

    /// <summary>
    /// Toggles whether or not gaze dots are instantiated.
    /// </summary>
    public void ToggleGazeDots()
    {
        showGazeDots = !showGazeDots;

        // Changes the color of the button when gaze dots are enabled and disabled
        if (showGazeDots) button.GetComponent<Image>().color = new Color(48f / 255, 174 / 255f, 253f / 255);
        else button.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    private void OnEnable()
    {
        if (ActionAsset != null)
        {
            ActionAsset.Enable();
        }
    }


    private void Update()
    {
        if (showGazeDots)
        {
            // Retrieves the gaze data needed to 
            var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
            var rayOrigin = eyeTrackingData.GazeRay.Origin;
            var rayDirection = eyeTrackingData.GazeRay.Direction;

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity) && hit.collider.tag != "Controller")
            {
                sphere = Instantiate(gazeSphere, hit.point, Quaternion.identity);
                StartCoroutine(deleteAfter(sphere));
            }
        }
    }

    /// <summary>
    /// Deletes an object after waiting for a set number of seconds. 
    /// Has to be run through a coroutine.
    /// </summary>
    /// <param name="objectToDelete">The object to delete after a delay.</param>
    /// <returns></returns>
    private IEnumerator deleteAfter(GameObject objectToDelete)
    {
        yield return new WaitForSeconds(0.1f);

        if (objectToDelete != null) Destroy(objectToDelete);
    }
}
