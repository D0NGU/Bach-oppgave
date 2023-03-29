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
            dataClass.time = so.speed;
            dataClass.hasMovement = so.hasMovement;
            saveToFile.AddObjectDataToList(dataClass);
        }
        saveToFile.SaveToJSON(inputField.text, overwrite);
        saveToFile = new();
    }
}
