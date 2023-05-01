using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectMenuController : MonoBehaviour
{
    public GameObject selectedObject;
    private SelectableObject selectableObject;

    [SerializeField]
    private GameObject leftControllerCanvas;
    [SerializeField]
    private Slider scaleSlider;
    [SerializeField]
    private TMP_Text editMovementText;

    [SerializeField]
    private GameObject editObjectCanvas;
    [SerializeField]
    private GameObject editMovementCanvas;
    [SerializeField]
    private GameObject wristMenus;

    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text startDelayText;
    [SerializeField]
    private TMP_Text visionDetectionTimeText;
    [SerializeField]
    private TMP_Text previewText;
    [SerializeField]
    private TMP_Text previewFullTestText;

    [SerializeField]
    private GameObject spherePrefab;
    [SerializeField]
    private Button removeMovementButton;
    [SerializeField]
    private Button saveMovementButton;
    [SerializeField]
    private Button loopMovementButton;
    [SerializeField]
    private GameObject testObjectParent;


    private void Start()
    {
        TestDataStatic.visionDetectionTime = 1.5f;

        UpdateVisionDetectionTimeDisplay();

    }


    public void ChangeSelectedObject(GameObject newSelectedObject)
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            if (selectedObject.GetComponent<SelectableObject>().move)
            {
                ToggleMovementPreview();
            }
        }

        selectedObject = newSelectedObject;
        if (selectedObject.GetComponent<SelectableObject>())
        {
            selectableObject = selectedObject.GetComponent<SelectableObject>();
        }

        showEditCanvas();
    }

    public void RemoveSelection()
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }

        selectedObject = null;
        selectableObject = null;

        showLeftControllerCanvas();
    }

    public void DeleteSelectedObject()
    {
        if (selectableObject != null) selectableObject.DeleteObject();

        showLeftControllerCanvas();

        selectedObject = null;
        selectableObject = null;
    }

    public void ChangeSphereType(string objectType)
    {
        if (objectType == "fullsphere") selectableObject.ChangeToFullSphere();
        else if (objectType == "lefthalf") selectableObject.ChangeToLeftHalf();
        else if (objectType == "righthalf") selectableObject.ChangeToRightHalf();
    }


    public void showEditCanvas()
    {
        leftControllerCanvas.SetActive(false);
        editMovementCanvas.SetActive(false);
        editObjectCanvas.SetActive(true);

        scaleSlider.value = selectableObject.scale.x;

        UpdateStartDelayDisplay();

        if (selectableObject.hasMovement) editMovementText.GetComponent<TextMeshProUGUI>().text = "Edit movement";
        else editMovementText.GetComponent<TextMeshProUGUI>().text = "Add movement";
    }

    public void showLeftControllerCanvas()
    {
        editMovementCanvas.SetActive(false);
        editObjectCanvas.SetActive(false);
        leftControllerCanvas.SetActive(true);
    }

    public void EditMovement()
    {
        selectableObject.EditMovement();
        showEditMovementCanvas();
        UpdateMovementTimeDisplay();

        if (selectableObject.loopMovement) loopMovementButton.GetComponent<Image>().color = new Color(0f / 255, 160f / 255, 255f / 255);
        else loopMovementButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void showEditMovementCanvas()
    {
        editObjectCanvas.SetActive(false);
        editMovementCanvas.SetActive(true);
    }

    public void ToggleMovementPreview()
    {

        selectableObject.StartTest();
        removeMovementButton.interactable = !removeMovementButton.interactable;
        saveMovementButton.interactable = !saveMovementButton.interactable;
        loopMovementButton.interactable = !loopMovementButton.interactable;

        if (selectableObject.testActive)
        {
            previewText.GetComponent<TextMeshProUGUI>().text = "Stop preview";
        }
        else
        {
            previewText.GetComponent<TextMeshProUGUI>().text = "Start preview";
        }
    }

    public void PreviewFullTest()
    {
        TestDataStatic.testIsRunning = !TestDataStatic.testIsRunning;

        wristMenus.SetActive(!TestDataStatic.testIsRunning);

        removeMovementButton.interactable = true;
        saveMovementButton.interactable = true;
        loopMovementButton.interactable = true;
        previewText.GetComponent<TextMeshProUGUI>().text = "Start preview";

        if (TestDataStatic.testIsRunning)
        {
            previewFullTestText.GetComponent<TextMeshProUGUI>().text = "Stop preview";

            foreach (Transform child in testObjectParent.transform)
            {
                SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
                if (!so.testActive)
                {
                    child.Find("Sphere").GetComponent<SelectableObject>().StartTest();
                }
            }
        }
        else
        {
            foreach (Transform child in testObjectParent.transform)
            {
                previewFullTestText.GetComponent<TextMeshProUGUI>().text = "Preview Test";

                SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
                if (so.testActive)
                {
                    child.Find("Sphere").GetComponent<SelectableObject>().StartTest();
                }
            }
        }
        
    }

    public void RemoveMovement()
    {
        selectableObject.RemoveMovement();
        showEditCanvas();
    }

    public void SaveMovement()
    {
        showEditCanvas();
    }

    public void ToogleLoopMovement()
    {
        selectableObject.ToggleLoopMovement();

        // Changes the color of the button when looped movement is enabled and disabled
        if (selectableObject.loopMovement) loopMovementButton.GetComponent<Image>().color = new Color(0f / 255, 160f / 255, 255f / 255);
        else loopMovementButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void SetObjectScale(float scale)
    {
        selectableObject.SetScale(scale);
    }


    public void IncreaseMovementTime()
    {
        selectableObject.moveTime += 0.5f;
        UpdateMovementTimeDisplay();
    }

    public void DecreaseMovementTime()
    {
        if (selectableObject.moveTime > 0.5f) selectableObject.moveTime -= 0.5f;
        UpdateMovementTimeDisplay();
    }

    public void UpdateMovementTimeDisplay()
    {
        timeText.GetComponent<TextMeshProUGUI>().text = selectableObject.moveTime.ToString() + "s";
    }


    public void IncreaseStartDelay()
    {
        selectableObject.startDelay += 0.5f;
        UpdateStartDelayDisplay();
    }

    public void DecreaseStartDelay()
    {
        if (selectableObject.startDelay > 0.0f) selectableObject.startDelay -= 0.5f;
        UpdateStartDelayDisplay();
    }

    public void UpdateStartDelayDisplay()
    {
        startDelayText.GetComponent<TextMeshProUGUI>().text = selectableObject.startDelay.ToString() + "s";
        Debug.Log(selectableObject.startDelay);
    }


    public void IncreaseVisionDetectionTime()
    {
        TestDataStatic.visionDetectionTime += 0.5f;
        UpdateVisionDetectionTimeDisplay();
    }

    public void DecreaseVisionDetectionTime()
    {
        if (TestDataStatic.visionDetectionTime > 0.5f) TestDataStatic.visionDetectionTime -= 0.5f;
        UpdateVisionDetectionTimeDisplay();
    }

    public void UpdateVisionDetectionTimeDisplay()
    {
        visionDetectionTimeText.GetComponent<TextMeshProUGUI>().text = TestDataStatic.visionDetectionTime + "s";
    }
}
