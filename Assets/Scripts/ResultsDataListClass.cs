using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class containing all result data of a test. 
/// </summary>
[System.Serializable]
public class ResultsDataListClass
{
    /// <summary>
    /// The time it takes to detect the players vision on an object
    /// </summary>
    public float visionDetectionTime = TestDataStatic.visionDetectionTime;
    /// <summary>
    /// The players distance to the test area
    /// </summary>
    public float playerDistance = TestDataStatic.playerDistance;

    public List<SelectableObjectResultsDataClass> selectableObjectsResultsDataList = new List<SelectableObjectResultsDataClass>();


    public MyEnumerator GetEnumerator()
    {
        return new MyEnumerator(this);
    }

    public class MyEnumerator
    {
        int nIndex;
        ResultsDataListClass collection;
        public MyEnumerator(ResultsDataListClass coll)
        {
            collection = coll;
            nIndex = -1;
        }

        public bool MoveNext()
        {
            nIndex++;
            return (nIndex < collection.selectableObjectsResultsDataList.Count);
        }

        public SelectableObjectResultsDataClass Current => collection.selectableObjectsResultsDataList[nIndex];
    }

    public void AddObjectDataToList(SelectableObjectResultsDataClass selectableObjectResultsDataClass)
    {
        selectableObjectsResultsDataList.Add(selectableObjectResultsDataClass);
    }

}

/// <summary>
/// Class containing all relevant data of a selectable object
/// when saving test results.
/// </summary>
[System.Serializable]
public class SelectableObjectResultsDataClass
{
    public Vector3 startPosistion;
    public Vector3 endPosistion;
    public Vector3 positionWhenSeen;
    public Vector3 scale;
    public float moveTime;
    public float startDelay;
    public bool hasMovement;
    public bool loopMovement;
    public string objectType;
    public bool hasBeenSeen;
    public float timePassedBeforeSeen;
}