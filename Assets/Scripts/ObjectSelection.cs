using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectSelection : MonoBehaviour
{
    public GameObject selectedObject;

    public GameObject editObjectCanvas;
    public GameObject leftControllerCanvas;
    public GameObject editMovementCanvas;

    public TMP_Text editMovementText;

    public void showEditCanvas()
    {
        leftControllerCanvas.SetActive(false);
        editMovementCanvas.SetActive(false);
        editObjectCanvas.SetActive(true);

        if (selectedObject.GetComponent<SelectableObject>().hasMovement) editMovementText.GetComponent<TextMeshProUGUI>().text = "Edit movement";
        else editMovementText.GetComponent<TextMeshProUGUI>().text = "Add movement";
    }

    public void showLeftControllerCanvas()
    {
        editMovementCanvas.SetActive(false);
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
        if (selectedObject != null) selectedObject.GetComponent<SelectableObject>().DeleteObject();

        showLeftControllerCanvas();

        selectedObject = null;
    }

   

}
