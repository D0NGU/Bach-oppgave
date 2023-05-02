using System.Collections.Generic;

/// <summary>
/// Class containing all data of a wave/set of a randomized test. 
/// </summary>
[System.Serializable]
public class RandomizedResultsDataClass
{
    /// <summary>
    /// The time it takes to detect the players vision on an object
    /// </summary>
    public float visionDetectionTime = TestDataStatic.visionDetectionTime;
    /// <summary>
    /// The players distance to the test area
    /// </summary>
    public float playerDistance = TestDataStatic.playerDistance;

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

/// <summary>
/// Class containing all the parameters of a randomized test.
/// </summary>
[System.Serializable]
public class RandomizedTestParametersClass
{
    public int amount;
    public int interval;
    public int percentFullSpheres;
    public int percentLeftSpheres;
    public string displayedSide;

}

/// <summary>
/// Class containing list of randomized test waves/sets
/// </summary>
[System.Serializable]
public class RandomizedResultsListClass
{
    public List<RandomizedResultsDataClass> list = new List<RandomizedResultsDataClass>();

}
