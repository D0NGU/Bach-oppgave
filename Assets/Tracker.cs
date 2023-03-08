using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tracker : MonoBehaviour
{

    [SerializeField] private InputActionReference eyePose;
    [SerializeField] private InputActionAsset ActionAsset;

    private void OnEnable()
    {
        if(ActionAsset != null)
        {
            ActionAsset.Enable();
        }

    }
    

    private void Update()
    {
        Debug.Log(eyePose.action.ReadValue<Vector3>());
    }
}
