using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObjectsScript
{
    public SelectableObjectDataListClass selectableObjectData = new();
    
    //TODO create folder if it doesnt exist from before in the dir
    public void SaveToJSON(string fileName, bool overwrite)
    {
        string dataJSONString = JsonUtility.ToJson(selectableObjectData);
        Debug.Log(dataJSONString);

        string filePath;
        if (overwrite) filePath = TestNameStatic.currentTestFilePath;
        else filePath = Application.dataPath + "/TestFiles/" + fileName.Replace(" ", "") + ".json"; 

        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, dataJSONString);
        Debug.Log("Save file created");
    }

    public void LoadFromJSON()
    {
        if (System.IO.File.Exists(TestNameStatic.testFilePath))
        {
            string dataJSONString = System.IO.File.ReadAllText(TestNameStatic.testFilePath);
            selectableObjectData = JsonUtility.FromJson<SelectableObjectDataListClass>(dataJSONString);
            Debug.Log("Test loaded");
        }
    }

    public void AddObjectDataToList(SelectableObjectDataClass selectableObjectDataClass)
    {

        selectableObjectData.selectableObjectsDataList.Add(selectableObjectDataClass);
    }


}
