using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class containing all selectable object of a created test
/// </summary>
[System.Serializable]
public class SelectableObjectDataListClass
{ 
    /// <summary>
    /// The time it takes to detect the players vision on an object
    /// </summary>
    public float visionDetectionTime = TestDataStatic.visionDetectionTime;

    public List<SelectableObjectDataClass> selectableObjectsDataList = new List<SelectableObjectDataClass>();


   public MyEnumerator GetEnumerator()
   {  
      return new MyEnumerator(this);
   }

    public class MyEnumerator
    {
        int nIndex;
        SelectableObjectDataListClass collection;
        public MyEnumerator(SelectableObjectDataListClass coll)
        {
            collection = coll;
            nIndex = -1;
        }

        public bool MoveNext()
        {
            nIndex++;
            return (nIndex < collection.selectableObjectsDataList.Count);
        }

        public SelectableObjectDataClass Current => collection.selectableObjectsDataList[nIndex];
    }  
}

/// <summary>
/// Class containing all relevant data of a selectable object
/// when saving a test.
/// </summary>
[System.Serializable]
public class SelectableObjectDataClass
{
    public Vector3 startPosistion;
    public Vector3 endPosistion;
    public Vector3 scale;
    public float moveTime;
    public float startDelay;
    public bool hasMovement;
    public bool loopMovement;
    public string objectType;
}
