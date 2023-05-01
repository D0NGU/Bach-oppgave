using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestSaver : MonoBehaviour
{

    [SerializeField]
    private GameObject testObjectParent;
    [SerializeField]
    private TMP_InputField inputField;

    public SaveObjectsScript saveToFile;

    public SaveObjectsScript saveRandomziedTestToFile;

    private void Awake()
    {
        saveToFile = new();
        saveRandomziedTestToFile = new();
    }
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
