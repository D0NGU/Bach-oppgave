using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private GameObject ObjectSelectionManager;

    // Start is called before the first frame update
    void Start()
    {
        ObjectSelectionManager = GameObject.Find("ObjectSelectionManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Selected()
    {
        // Checks if this gameobject is already selected
        if (!GameObject.ReferenceEquals(ObjectSelectionManager.GetComponent<ObjectSelection>().selectedObject, this.gameObject))
        {

            ObjectSelectionManager.GetComponent<ObjectSelection>().ChangeSelectedObject(this.gameObject);

            this.gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        }

        Debug.Log("Select");
    }

    public void DeSelected()
    {
        Debug.Log("Deselect");
    }
}
