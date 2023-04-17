using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectMenuController : MonoBehaviour
{
    public ObjectSelection objectSelection;

    public GameObject editObjectCanvas;
    public GameObject editMovementCanvas;

    public TMP_Text timeText;
    public TMP_Text visionDetectionTimeText;
    public TMP_Text previewText;

    public Button removeMovementButton;
    public Button saveMovementButton;
    public Button loopMovementButton;


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

        if (objectSelection.selectedObject.GetComponent<SelectableObject>().testActive)
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
        objectSelection.selectedObject.GetComponent<SelectableObject>().speed += 0.5f;
        UpdateMovementTimeDisplay();
    }

    public void DecreaseMovementTime()
    {
        if (objectSelection.selectedObject.GetComponent<SelectableObject>().speed > 0.5f) objectSelection.selectedObject.GetComponent<SelectableObject>().speed -= 0.5f;
        UpdateMovementTimeDisplay();
    }

    public void UpdateMovementTimeDisplay()
    {
        timeText.GetComponent<TextMeshProUGUI>().text = objectSelection.selectedObject.GetComponent<SelectableObject>().speed.ToString() + "s";
    }


    public void IncreaseVisionDetectionTime()
    {
        objectSelection.selectedObject.GetComponent<SelectableObject>().visionDetectionTime += 0.5f;
        UpdateVisionDetectionTimeDisplay();
    }

    public void DecreaseVisionDetectionTime()
    {
        if (objectSelection.selectedObject.GetComponent<SelectableObject>().visionDetectionTime > 0.5f) objectSelection.selectedObject.GetComponent<SelectableObject>().visionDetectionTime -= 0.5f;
        UpdateVisionDetectionTimeDisplay();
    }

    public void UpdateVisionDetectionTimeDisplay()
    {
        visionDetectionTimeText.GetComponent<TextMeshProUGUI>().text = objectSelection.selectedObject.GetComponent<SelectableObject>().visionDetectionTime.ToString() + "s";
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
}
