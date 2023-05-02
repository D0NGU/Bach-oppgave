using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for saving test results for normal and randomized tests.
/// </summary>
public class SaveResultsScript
{

    public RandomizedResultsListClass randomizedResultsList = new();

    /// <summary>
    /// Saves test results from a normal test to a json file
    /// </summary>
    /// <param name="filePath">Path of the file to save</param>
    /// <param name="resultsData">The result data</param>
    public void SaveToJSON(string filePath, ResultsDataListClass resultsData)
    {
        string dataJSONString = JsonUtility.ToJson(resultsData);

        System.IO.File.WriteAllText(filePath, dataJSONString);
        Debug.Log("Save file created");
    }

    /// <summary>
    /// Loads test results from a normal test from a json file
    /// </summary>
    /// <param name="filePath">Path of the file to load</param>
    /// <returns>Class containing all result data of a test</returns>
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


    /// <summary>
    /// Saves test results from a randomized test to a json file
    /// </summary>
    /// <param name="filePath">Path of the file to save</param>
    public void SaveRandomizedToJSON(string filePath)
    {
        string dataJSONString = JsonUtility.ToJson(randomizedResultsList);

        System.IO.File.WriteAllText(filePath, dataJSONString);
        Debug.Log("Save file created");
    }

    /// <summary>
    /// Loads test results from a randomized test from a json file
    /// </summary>
    /// <param name="filePath">Path of the file to load</param>
    /// <returns>List of all waves/sets of a randomized test</returns>
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