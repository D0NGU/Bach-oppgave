using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Tobii.XR;

public class GazeDots : MonoBehaviour
{
    [SerializeField] 
    private InputActionAsset ActionAsset;
    [SerializeField]
    private Button button;

    public GameObject gazeSphere;
    public bool showGazeDots = false;

    public void ToggleGazeDots()
    {
        showGazeDots = !showGazeDots;

        // Changes the color of the button when gaze dots are enabled and disabled
        if (showGazeDots) button.GetComponent<Image>().color = new Color(0f / 255, 160f / 255, 255f / 255);
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
            var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
            var rayOrigin = eyeTrackingData.GazeRay.Origin;
            var rayDirection = eyeTrackingData.GazeRay.Direction;

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity) && hit.collider.tag != "Controller")
            {
                GameObject sphere = Instantiate(gazeSphere, hit.point, Quaternion.identity);
                StartCoroutine(deleteAfter(sphere));
            }
        }
    }


    private IEnumerator deleteAfter(GameObject objectToDelete)
    {
        yield return new WaitForSeconds(0.1f);

        if (objectToDelete != null) Destroy(objectToDelete);
    }
}
