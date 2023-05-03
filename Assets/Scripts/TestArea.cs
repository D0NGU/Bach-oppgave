using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for handling functionality related to the
/// test area.
/// </summary>
public class TestArea : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The prefab of the selectable object")]
    private GameObject spherePrefab;
    [SerializeField]
    [Tooltip("The parent object of the selectable objects in the test area")]
    private GameObject testObjectParent;


    [Tooltip("Game object that marks the location of the top right back corner of the test area")]
    public GameObject topRightBackCorner;
    [Tooltip("Game object that marks the location of the bottom right front corner of the test area")]
    public GameObject bottomRightFrontCorner;
    [Tooltip("Game object that marks the location of the bottom left back corner of the test area")]
    public GameObject bottomLeftBackCorner;

    /// <summary>
    /// Instantiates a sphere prefab in the test area
    /// </summary>
    public void Spawn()
    {
        Instantiate(spherePrefab, testObjectParent.transform);
    }

    /// <summary>
    /// Gets a dictionary of the min and max x, y, z coordinates of the test area
    /// </summary>
    /// <returns>Dictionary containing the coordinates</returns>
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

    /// <summary>
    /// Hides/shows all game objects in the right half of the test area
    /// </summary>
    /// <param name="toggle">true (hide) or false (show)</param>
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

    /// <summary>
    /// Hides/shows all game objects in the left half of the test area
    /// </summary>
    /// <param name="toggle">true (hide) or false (show)</param>
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

    /// <summary>
    /// Shows all game objects in the test area
    /// </summary>
    public void ShowAllChildren()
    {
        foreach (Transform child in testObjectParent.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
