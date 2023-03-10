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

    private void OnEnable()
    {
        if(ActionAsset != null)
        {
            ActionAsset.Enable();
        }

    }
    

    private void Update()
    {

        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
        var rayOrigin = eyeTrackingData.GazeRay.Origin;
        var rayDirection = eyeTrackingData.GazeRay.Direction;
        

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity))
        {
            GameObject sphere = Instantiate(gazeSphere, hit.transform.position, Quaternion.identity);
            StartCoroutine(deleteAfter(sphere));
            
        }
          
        Debug.Log(hit.transform.position.ToString());

    }

    private IEnumerator deleteAfter(GameObject objectToDelete)
    {

        yield return new WaitForSeconds(0.1f);

        if (objectToDelete != null) Destroy(objectToDelete);
    }
}
