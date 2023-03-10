using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{

    public GameObject selectedObject;

    public GameObject editObjectCanvas;
    public GameObject leftControllerCanvas;


    public void showEditCanvas()
    {
        leftControllerCanvas.SetActive(false);
        editObjectCanvas.SetActive(true); 
    }

    public void showLeftControllerCanvas()
    {
        editObjectCanvas.SetActive(false);
        leftControllerCanvas.SetActive(true);
    }

    public void ChangeSelectedObject(GameObject newSelectedObject)
    {
        if (selectedObject != null) selectedObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");

        selectedObject = newSelectedObject;

        showEditCanvas();
    }

    public void RemoveSelection()
    {
        selectedObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        selectedObject = null;

        showLeftControllerCanvas();
    }

    public void DeleteSelectedObject()
    {
        if (selectedObject != null) Destroy(selectedObject);

        showLeftControllerCanvas();

        selectedObject = null;
    }
}
