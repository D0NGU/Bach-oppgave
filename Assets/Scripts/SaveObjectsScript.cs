using UnityEngine;

/// <summary>
/// Class responsible for saving and loading a test json file.
/// </summary>
public class SaveObjectsScript
{
    public SelectableObjectDataListClass selectableObjectData = new();
    
    /// <summary>
    /// Saves a json file containing a test
    /// </summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="overwrite">Whether to overwrite an existing file</param>
    public void SaveToJSON(string fileName, bool overwrite)
    {
        string dataJSONString = JsonUtility.ToJson(selectableObjectData);

        string filePath;
        if (overwrite) filePath = TestDataStatic.currentTestFilePath;
        else filePath = TestDataStatic.testFolderPath + fileName.Replace(" ", "") + ".json";

        TestDataStatic.currentTestFilePath = filePath;

        System.IO.File.WriteAllText(filePath, dataJSONString);
        Debug.Log("Save file created");
    }

    /// <summary>
    /// Loads a json file containing a test
    /// </summary>
    public void LoadFromJSON()
    {
        if (System.IO.File.Exists(TestDataStatic.testFilePath))
        {
            string dataJSONString = System.IO.File.ReadAllText(TestDataStatic.testFilePath);
            selectableObjectData = JsonUtility.FromJson<SelectableObjectDataListClass>(dataJSONString);
            Debug.Log("Test loaded");
        }
    }

    public void AddObjectDataToList(SelectableObjectDataClass selectableObjectDataClass)
    {
        selectableObjectData.selectableObjectsDataList.Add(selectableObjectDataClass);
    }


}
