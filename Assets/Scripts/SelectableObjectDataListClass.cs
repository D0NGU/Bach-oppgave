using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class SelectableObjectDataListClass
{
    public List<SelectableObjectDataClass> selectableObjectsDataList = new List<SelectableObjectDataClass>();

  

   public MyEnumerator GetEnumerator()
   {  
      return new MyEnumerator(this);
}

// Declare the enumerator class:  
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
[System.Serializable]
public class SelectableObjectDataClass
{
    public Vector3 startPosistion;
    public Vector3 endPosistion;
    public float time;
    public bool hasMovement;
}
