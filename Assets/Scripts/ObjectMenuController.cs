using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectMenuController : MonoBehaviour
{
    [SerializeField]
    private ObjectSelection objectSelection;


    [SerializeField]
    private GameObject editObjectCanvas;
    [SerializeField]
    private GameObject editMovementCanvas;

    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text startDelayText;
    [SerializeField]
    private TMP_Text visionDetectionTimeText;
    [SerializeField]
    private TMP_Text previewText;


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


    public void EditMovement()
    {
        objectSelection.selectedObject.GetComponent<SelectableObject>().EditMovement();
        showEditMovementCanvas();
        UpdateMovementTimeDisplay();

        if (objectSelection.selectedObject.GetComponent<SelectableObject>().loopMovement) loopMovementButton.GetComponent<Image>().color = new Color(0f / 255, 160f / 255, 255f / 255);
        else loopMovementButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void showEditMovementCanvas()
    {
        editObjectCanvas.SetActive(false);
        editMovementCanvas.SetActive(true);
    }

    public void Preview()
    {

        objectSelection.selectedObject.GetComponent<SelectableObject>().StartTest();
        removeMovementButton.interactable = !removeMovementButton.interactable;
        saveMovementButton.interactable = !saveMovementButton.interactable;
        loopMovementButton.interactable = !loopMovementButton.interactable;

        if (TestDataStatic.testIsRunning)
        {
            previewText.GetComponent<TextMeshProUGUI>().text = "Stop preview";
        }
        else
        {
            previewText.GetComponent<TextMeshProUGUI>().text = "Start preview";
        }
    }


    public void IncreaseMovementTime()
    {
        objectSelection.selectedObject.GetComponent<SelectableObject>().moveTime += 0.5f;
        UpdateMovementTimeDisplay();
    }

    public void DecreaseMovementTime()
    {
        if (objectSelection.selectedObject.GetComponent<SelectableObject>().moveTime > 0.5f) objectSelection.selectedObject.GetComponent<SelectableObject>().moveTime -= 0.5f;
        UpdateMovementTimeDisplay();
    }

    public void UpdateMovementTimeDisplay()
    {
        timeText.GetComponent<TextMeshProUGUI>().text = objectSelection.selectedObject.GetComponent<SelectableObject>().moveTime.ToString() + "s";
    }


    public void IncreaseStartDelay()
    {
        objectSelection.selectedObject.GetComponent<SelectableObject>().startDelay += 0.5f;
        UpdateStartDelayDisplay();
    }

    public void DecreaseStartDelay()
    {
        if (objectSelection.selectedObject.GetComponent<SelectableObject>().startDelay > 0.0f) objectSelection.selectedObject.GetComponent<SelectableObject>().startDelay -= 0.5f;
        UpdateStartDelayDisplay();
    }

    public void UpdateStartDelayDisplay()
    {
        startDelayText.GetComponent<TextMeshProUGUI>().text = objectSelection.selectedObject.GetComponent<SelectableObject>().startDelay.ToString() + "s";
        Debug.Log(objectSelection.selectedObject.GetComponent<SelectableObject>().startDelay);
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


    public void RemoveMovement()
    {
        objectSelection.selectedObject.GetComponent<SelectableObject>().RemoveMovement();
        objectSelection.showEditCanvas();
    }

    public void SaveMovement()
    {
        objectSelection.showEditCanvas();
    }

    public void ToogleLoopMovement()
    {
        objectSelection.selectedObject.GetComponent<SelectableObject>().ToggleLoopMovement();
        if (objectSelection.selectedObject.GetComponent<SelectableObject>().loopMovement) loopMovementButton.GetComponent<Image>().color = new Color(0f / 255, 160f / 255, 255f / 255);
        else loopMovementButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void SetObjectScale(float scale)
    {
        objectSelection.selectedObject.GetComponent<SelectableObject>().SetScale(scale);
    }
}
