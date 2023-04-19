using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveResultsScript
{
    public ResultsDataListClass resultsData = new();

    //TODO create folder if it doesnt exist from before in the dir
    public void SaveToJSON(string filePath)
    {
        string dataJSONString = JsonUtility.ToJson(resultsData);

        System.IO.File.WriteAllText(filePath, dataJSONString);
        Debug.Log("Save file created");
    }

    public void LoadFromJSON(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            string dataJSONString = System.IO.File.ReadAllText(filePath);
            resultsData = JsonUtility.FromJson<ResultsDataListClass>(dataJSONString);
            Debug.Log("Test loaded");
        }
    }

    public void AddObjectDataToList(SelectableObjectResultsDataClass selectableObjectResultsDataClass)
    {
        resultsData.selectableObjectsResultsDataList.Add(selectableObjectResultsDataClass);
    }


}