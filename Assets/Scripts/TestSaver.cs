using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestSaver : MonoBehaviour
{

    public GameObject testObjectParent;
    public SaveObjectsScript saveToFile = new();
    public TMP_InputField inputField;

    public void SaveTest(bool overwrite)
    {
        foreach (Transform child in testObjectParent.transform)
        {
            SelectableObjectDataClass dataClass = new();
            SelectableObject so = child.Find("Sphere").GetComponent<SelectableObject>();
            dataClass.startPosistion = so.startPos;
            dataClass.endPosistion = so.endPos;
            dataClass.scale = so.scale;
            dataClass.time = so.speed;
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
