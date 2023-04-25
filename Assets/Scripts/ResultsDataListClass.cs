using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class ResultsDataListClass
{
    public float visionDetectionTime = TestDataStatic.visionDetectionTime;
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