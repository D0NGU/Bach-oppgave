using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private GameObject objectSelectionController;
    

    void Start()
    {
        objectSelectionController = GameObject.Find("ObjectSelectionController");
    }
    

    public void Selected()
    {
        // Checks if this gameobject is already selected
        if (!GameObject.ReferenceEquals(objectSelectionController.GetComponent<ObjectSelection>().selectedObject, this.gameObject))
        {

            objectSelectionController.GetComponent<ObjectSelection>().ChangeSelectedObject(this.gameObject);

            this.gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        }

        Debug.Log("Select");
    }

    public void DeSelected()
    {
        Debug.Log("Deselect");
    }
}
