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
        if(selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }
        
        selectedObject = null;

        showLeftControllerCanvas();
    }

    public void DeleteSelectedObject()
    {
        if (selectedObject != null) selectedObject.GetComponent<SelectableObject>().DeleteObject();

        showLeftControllerCanvas();

        selectedObject = null;
    }

    public void ChangeSphereType(string objectType)
    {
        if (objectType == "fullsphere") selectedObject.GetComponent<SelectableObject>().ChangeToFullSphere();
        else if (objectType == "lefthalf") selectedObject.GetComponent<SelectableObject>().ChangeToLeftHalf();
        else if (objectType == "righthalf") selectedObject.GetComponent<SelectableObject>().ChangeToRightHalf();
    }

}
