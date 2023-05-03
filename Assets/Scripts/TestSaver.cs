using UnityEngine;
using TMPro;

/// <summary>
/// Responsible for saving a created test
/// </summary>
public class TestSaver : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The parent object of the selectable objects in the test area")]
    private GameObject testObjectParent;
    [SerializeField]
    [Tooltip("TMPRo input field that holds the name of the test")]
    private TMP_InputField inputField;

    public SaveObjectsScript saveToFile;

    private void Awake()
    {
        saveToFile = new();
    }

    /// <summary>
    /// Saves a created test to a json file
    /// </summary>
    /// <param name="overwrite">Whether to overwrite and existing file</param>
    public void SaveTest(bool overwrite)
    {
        foreach (Transform child in testObjectParent.transform)
        {
            SelectableObjectDataClass dataClass = new();
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            dataClass.startPosistion = so.startPos;
            dataClass.endPosistion = so.endPos;
            dataClass.scale = so.scale;
            dataClass.moveTime = so.moveTime;
            dataClass.startDelay = so.startDelay;
            dataClass.hasMovement = so.hasMovement;
            dataClass.loopMovement = so.loopMovement;
            dataClass.objectType = so.objectType;

            saveToFile.AddObjectDataToList(dataClass);
        }

        saveToFile.selectableObjectData.visionDetectionTime = TestDataStatic.visionDetectionTime;

        saveToFile.SaveToJSON(inputField.text, overwrite);
        saveToFile = new();
    }

}
