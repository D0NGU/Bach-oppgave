using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Responsible for object selection and all UI options 
/// for editing a <see cref="SelectableObject"/> 
/// </summary>
public class ObjectMenuController : MonoBehaviour
{
    /// <summary>
    /// The object currently selected with the XR Ray Interactor
    /// </summary>
    public GameObject selectedObject;
    /// <summary>
    /// The <see cref="SelectableObject"/> component of the selected object
    /// </summary>
    private SelectableObject selectableObject;

    [SerializeField]
    [Tooltip("Main wrist UI canvas with general options")]
    private GameObject leftControllerCanvas;
    [SerializeField]
    [Tooltip("UI slider for changing the selected objects scale")]
    private Slider scaleSlider;
    [SerializeField]
    [Tooltip("TMPro text of the button for editing a selected objects movement")]
    private TMP_Text editMovementText;

    [SerializeField]
    [Tooltip("UI canvas with options for changing general parameters of a selected object")]
    private GameObject editObjectCanvas;
    [SerializeField]
    [Tooltip("UI canvas with options for changing parameters of a selected object related to movement")]
    private GameObject editMovementCanvas;
    [SerializeField]
    [Tooltip("Parent object of all the wrist UI menus")]
    private GameObject wristMenus;

    [SerializeField]
    [Tooltip("TMPro text displaying the movement time of the selected object")]
    private TMP_Text movementTimeText;
    [SerializeField]
    [Tooltip("TMPro text displaying the start delay of the selected object")]
    private TMP_Text startDelayText;
    [SerializeField]
    [Tooltip("TMPro text displaying the vision detection time of the test")]
    private TMP_Text visionDetectionTimeText;
    [SerializeField]
    [Tooltip("TMPro text of the movement preview button")]
    private TMP_Text previewText;
    [SerializeField]
    [Tooltip("TMPro text of the full test preview button")]
    private TMP_Text previewFullTestText;

    [SerializeField]
    [Tooltip("Prefab of selectable object")]
    private GameObject spherePrefab;
    [SerializeField]
    [Tooltip("UI button for removing a selected objects movement")]
    private Button removeMovementButton;
    [SerializeField]
    [Tooltip("UI button for saving a selected objects movement")]
    private Button saveMovementButton;
    [SerializeField]
    [Tooltip("UI button for toggling the movement looping property of the selected object")]
    private Button loopMovementButton;
    [SerializeField]
    [Tooltip("Parent object of all selectable objects in the editor test area")]
    private GameObject testObjectParent;


    private void Start()
    {
        TestDataStatic.visionDetectionTime = 1.5f;

        UpdateVisionDetectionTimeDisplay();

    }

    /// <summary>
    /// Changes the object selection and shows the manu for editing the
    /// selected object. 
    /// </summary>
    /// <param name="newSelectedObject">New selected object</param>
    public void ChangeSelectedObject(GameObject newSelectedObject)
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");

            // Stops the movement preview of the selected object before changing selection. 
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

    /// <summary>
    /// Removes the current selection and shows the main left controller menu
    /// </summary>
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

    /// <summary>
    /// Deletes the currently selected object.
    /// </summary>
    public void DeleteSelectedObject()
    {
        if (selectableObject != null) selectableObject.DeleteObject();

        showLeftControllerCanvas();

        selectedObject = null;
        selectableObject = null;
    }

    /// <summary>
    /// Changes the sphere type of the selected object.
    /// </summary>
    /// <param name="objectType">The type of object to change to. Either "fullsphere", "lefthalf", or "righthalf"</param>
    public void ChangeSphereType(string objectType)
    {
        if (objectType == "fullsphere") selectableObject.ChangeToFullSphere();
        else if (objectType == "lefthalf") selectableObject.ChangeToLeftHalf();
        else if (objectType == "righthalf") selectableObject.ChangeToRightHalf();
    }

    /// <summary>
    /// Shows the menu for editing the main parameters of the selected object.
    /// </summary>
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

    /// <summary>
    /// Shows the main left controller canvas with general options
    /// for the editor.
    /// </summary>
    public void showLeftControllerCanvas()
    {
        editMovementCanvas.SetActive(false);
        editObjectCanvas.SetActive(false);
        leftControllerCanvas.SetActive(true);
    }

   /// <summary>
   /// Shows the movement menu and adds movement to the selected object.
   /// </summary>
    public void EditMovement()
    {
        selectableObject.AddMovement();
        showEditMovementCanvas();
        UpdateMovementTimeDisplay();

        if (selectableObject.loopMovement) loopMovementButton.GetComponent<Image>().color = new Color(0f / 255, 160f / 255, 255f / 255);
        else loopMovementButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    /// <summary>
    /// Shows the menu for editing parameters related to the selected objects movement.
    /// </summary>
    public void showEditMovementCanvas()
    {
        editObjectCanvas.SetActive(false);
        editMovementCanvas.SetActive(true);
    }

    /// <summary>
    /// Start/stops a preview of how the object will move during a test.
    /// </summary>
    public void ToggleMovementPreview()
    {

        selectableObject.StartTest();

        // Changes the interactability of buttons to prevent them from being used when the preview is active.
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

    /// <summary>
    /// Start/stops a preview of the entire test currently worked on in the editor.
    /// </summary>
    public void PreviewFullTest()
    {
        // If the selected object is currently in a movement preview, it is stopped first.
        if (selectableObject.testActive && !TestDataStatic.testIsRunning)
        {
            selectableObject.StartTest();

            removeMovementButton.interactable = true;
            saveMovementButton.interactable = true;
            loopMovementButton.interactable = true;
            previewText.GetComponent<TextMeshProUGUI>().text = "Start preview";
        }

        TestDataStatic.testIsRunning = !TestDataStatic.testIsRunning;
        // Hides all the wrist menus to prevent any potential issues caused by 
        // changes of parameters while the preview is running.
        wristMenus.SetActive(!TestDataStatic.testIsRunning);


        if (TestDataStatic.testIsRunning)
        {
            previewFullTestText.GetComponent<TextMeshProUGUI>().text = "Stop preview";
        }
        else
        {
            previewFullTestText.GetComponent<TextMeshProUGUI>().text = "Preview Test";
        }

        // Start/stops the preview for all test object in the scene.
        foreach (Transform child in testObjectParent.transform)
        {
            child.Find("Sphere").GetComponent<SelectableObject>().StartTest();
        }

    }

    /// <summary>
    /// Hides the movement menu and removes movement from the selected object.
    /// </summary>
    public void RemoveMovement()
    {
        selectableObject.RemoveMovement();
        showEditCanvas();
    }

    public void SaveMovement()
    {
        showEditCanvas();
    }

    /// <summary>
    /// Toggle for the movement looping property of the selected object.
    /// </summary>
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
        movementTimeText.GetComponent<TextMeshProUGUI>().text = selectableObject.moveTime.ToString() + "s";
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
