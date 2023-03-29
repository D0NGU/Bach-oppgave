using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Tobii.XR;

public class TestStarter : MonoBehaviour
{
    private Coroutine coroutine;
    private bool testIsRunning = false;
    private List<Vector3> eyeGazeData = new();
    
    public int time = 3;
    public TMP_Text countdownText;
    public TMP_Text startButtonText;
    public GameObject testObjectParent;


    private void Update()
    {
        if (testIsRunning)
        {
            var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
            if (eyeTrackingData.GazeRay.IsValid)
            {
                var rayOrigin = eyeTrackingData.GazeRay.Origin;
                var rayDirection = eyeTrackingData.GazeRay.Direction;
                RaycastHit hit;
                Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity);
                eyeGazeData.Add(hit.point);
            }
        }
    }

    public void StartCountdown()
    {
        if (coroutine != null) { 
            StopCoroutine(coroutine);
            coroutine = null;
            countdownText.gameObject.SetActive(false);
            testObjectParent.SetActive(true);
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
        }
        else if(!testIsRunning){
            testObjectParent.SetActive(false);
            coroutine = StartCoroutine(CountDown());
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Stop Test";
        }
        else {
            StartTest();
            startButtonText.GetComponent<TextMeshProUGUI>().text = "Start Test";
        }
    }

    public IEnumerator CountDown()
    {
        int localTime = time;
        countdownText.GetComponent<TextMeshProUGUI>().text = localTime.ToString();
        countdownText.gameObject.SetActive(true);

        while (localTime > 0)
        {
            Debug.Log(localTime);
            yield return new WaitForSeconds(1);
            localTime--;
            countdownText.GetComponent<TextMeshProUGUI>().text = localTime.ToString();
        }
        countdownText.gameObject.SetActive(false);
        testObjectParent.SetActive(true);
        StartTest();
    }

    public void StartTest()
    {
        eyeGazeData.Clear();
        testIsRunning = !testIsRunning;
        coroutine = null;
        foreach (Transform child in testObjectParent.transform)
        {
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            if (so.hasMovement) so.Preview();
        }
        
       

    }

    public void SaveGazeData()
    {

    }


}
