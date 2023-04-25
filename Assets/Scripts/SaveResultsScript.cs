using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveResultsScript
{

    public RandomizedResultsListClass randomizedResultsList = new();



    public void SaveToJSON(string filePath, ResultsDataListClass resultsData)
    {
        string dataJSONString = JsonUtility.ToJson(resultsData);

        System.IO.File.WriteAllText(filePath, dataJSONString);
        Debug.Log("Save file created");
    }

    public ResultsDataListClass LoadFromJSON(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            string dataJSONString = System.IO.File.ReadAllText(filePath);
            ResultsDataListClass resultsData = JsonUtility.FromJson<ResultsDataListClass>(dataJSONString);
            Debug.Log("Test loaded");

            return resultsData;
        }

        return null;
    }



    public void SaveRandomizedToJSON(string filePath)
    {
        string dataJSONString = JsonUtility.ToJson(randomizedResultsList);

        System.IO.File.WriteAllText(filePath, dataJSONString);
        Debug.Log("Save file created");
    }

    public List<RandomizedResultsDataClass> LoadRandomizedFromJSON(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            string dataJSONString = System.IO.File.ReadAllText(filePath);
            List<RandomizedResultsDataClass> resultsData = JsonUtility.FromJson<List<RandomizedResultsDataClass>>(dataJSONString);
            Debug.Log("Test loaded");

            return resultsData;
        }

        return null;
    }


    public void AddToRandomizedResultsList(RandomizedResultsDataClass data)
    {
        randomizedResultsList.list.Add(data);
    }

    public void ClearRandomizedResultsList()
    {
        randomizedResultsList.list.Clear();
    }

}