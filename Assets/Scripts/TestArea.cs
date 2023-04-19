using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArea : MonoBehaviour
{

    [SerializeField]
    private GameObject spherePrefab;
    [SerializeField]
    private GameObject testObjectParent;


    public GameObject topRightBackCorner;
    public GameObject bottomRightFrontCorner;
    public GameObject bottomLeftBackCorner;


    public void Spawn()
    {
        Instantiate(spherePrefab, testObjectParent.transform);
    }

    
    public Dictionary<string, float> GetTestAreaBounds()
    {
        var dict = new Dictionary<string, float>(){
                    {"minX", bottomLeftBackCorner.transform.position.x},
                    {"maxX", topRightBackCorner.transform.position.x},
                    {"minY", bottomRightFrontCorner.transform.position.y},
                    {"maxY", topRightBackCorner.transform.position.y},
                    {"minZ", bottomRightFrontCorner.transform.position.z },
                    {"maxZ", bottomLeftBackCorner.transform.position.z}
        };

        return dict;
    }

    public void HideRightSide(bool toggle)
    {
        foreach (Transform child in testObjectParent.transform)
        {
            if (child.Find("Sphere").transform.position.x >= 0)
            {
                if (toggle)
                {
                    child.gameObject.SetActive(!child.gameObject.activeSelf);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    public void HideLeftSide(bool toggle)
    {
        foreach (Transform child in testObjectParent.transform)
        {
            if (child.Find("Sphere").transform.position.x < 0)
            {
                if (toggle)
                {
                    child.gameObject.SetActive(!child.gameObject.activeSelf);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ShowAllChildren()
    {
        foreach (Transform child in testObjectParent.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
