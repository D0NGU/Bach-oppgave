using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoader : MonoBehaviour
{

    public GameObject spheresPrefab;
    public GameObject testObjectParent;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "RunTest") LoadTest();
    }


    public void LoadTest()
    {
        foreach (Transform child in testObjectParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        SaveObjectsScript loadedData = new();
        loadedData.LoadFromJSON();
        foreach (SelectableObjectDataClass dataClass in loadedData.selectableObjectData)
        {
            GameObject o = Instantiate(spheresPrefab, testObjectParent.transform);
            SelectableObject so = o.transform.Find("Sphere").GetComponent<SelectableObject>();

            // Sets the position of the prefab
            so.gameObject.transform.position = dataClass.startPosistion;
            o.transform.Find("Ghost Sphere").transform.position = dataClass.endPosistion;

            // Sets public variables in script to correct values
            so.startPos = dataClass.startPosistion;
            so.endPos = dataClass.endPosistion;
            so.speed = dataClass.time;
            so.hasMovement = dataClass.hasMovement;

            so.ShowGhostSphere(so.hasMovement);
        }
    }

}
