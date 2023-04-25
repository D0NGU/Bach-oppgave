using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDistanceController : MonoBehaviour
{
    [SerializeField]
    private GameObject origin;

    private void Awake()
    {
        origin = GameObject.Find("XR Origin");

        TestDataStatic.playerDistance = default(float);
    }

    public void SetPlayerDistance(float distance)
    {
        TestDataStatic.playerDistance = distance;
        origin.transform.position = new Vector3(origin.transform.position.x, origin.transform.position.y, distance);
    }

}
