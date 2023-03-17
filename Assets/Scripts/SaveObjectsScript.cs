using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObjectsScript
{
    public SelectableObjectDataListClass selectableObjectData = new();
    
    public void SaveToJSON()
    {
        string dataJSONString = JsonUtility.ToJson(selectableObjectData);
        Debug.Log(dataJSONString);
        string filePath = Application.dataPath + "/ObejectData.json"; 
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, dataJSONString);
        Debug.Log("Save file created");
    }

    public void LoadFromJSON()
    {
        string filePath = Application.dataPath + "/ObejectData.json";
        

        if (System.IO.File.Exists(filePath))
        {
            string dataJSONString = System.IO.File.ReadAllText(filePath);
            selectableObjectData = JsonUtility.FromJson<SelectableObjectDataListClass>(dataJSONString);
            Debug.Log("Save loaded");
        }
    }

    public void AddObjectDataToList(SelectableObjectDataClass selectableObjectDataClass)
    {

        selectableObjectData.selectableObjectsDataList.Add(selectableObjectDataClass);
    }


}
