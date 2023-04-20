using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class RandomizedResultsDataClass
{
    public float visionDetectionTime = TestDataStatic.visionDetectionTime;

    public RandomizedTestParametersClass randomizedTestData = new();

    public List<SelectableObjectResultsDataClass> selectableObjectsResultsDataList = new List<SelectableObjectResultsDataClass>();


    public MyEnumerator GetEnumerator()
    {
        return new MyEnumerator(this);
    }

    public class MyEnumerator
    {
        int nIndex;
        RandomizedResultsDataClass collection;
        public MyEnumerator(RandomizedResultsDataClass coll)
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
public class RandomizedTestParametersClass
{
    public int amount;
    public int interval;
    public int percentFullSpheres;
    public int percentLeftSpheres;
    public string displayedSide;

}

[System.Serializable]
public class RandomizedResultsListClass
{
    public List<RandomizedResultsDataClass> list = new List<RandomizedResultsDataClass>();

}
