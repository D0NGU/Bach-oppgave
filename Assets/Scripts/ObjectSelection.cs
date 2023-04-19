using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectSelection : MonoBehaviour
{
    public GameObject selectedObject;

    [SerializeField]
    private ObjectMenuController objectMenuController;
    [SerializeField]
    private GameObject editObjectCanvas;
    [SerializeField]
    private GameObject leftControllerCanvas;
    [SerializeField]
    private GameObject editMovementCanvas;
    [SerializeField]
    private Slider scaleSlider;
    [SerializeField]
    private TMP_Text editMovementText;

    public void showEditCanvas()
    {
        leftControllerCanvas.SetActive(false);
        editMovementCanvas.SetActive(false);
        editObjectCanvas.SetActive(true);

        scaleSlider.value = selectedObject.GetComponent<SelectableObject>().scale.x;

        objectMenuController.UpdateStartDelayDisplay();
        Debug.Log(selectedObject.GetComponent<SelectableObject>().startDelay);

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
