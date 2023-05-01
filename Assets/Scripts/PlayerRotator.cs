using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField]
    private GameObject VRCamera;

    public void RotatePerspetive()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0f, transform.rotation.eulerAngles.z);
        var vrCamAngle = Quaternion.Inverse(VRCamera.transform.rotation);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, vrCamAngle.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}


