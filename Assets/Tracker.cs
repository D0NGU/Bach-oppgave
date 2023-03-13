using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Tobii.XR;

public class Tracker : MonoBehaviour
{

    [SerializeField] private InputActionReference eyePose;
    [SerializeField] private InputActionAsset ActionAsset;

    public GameObject gazeSphere;
    public WristUI wristUI;

    private void OnEnable()
    {
        if(ActionAsset != null)
        {
            ActionAsset.Enable();
        }
    }
    

    private void Update()
    {
        if (wristUI.showGazeDots)
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
