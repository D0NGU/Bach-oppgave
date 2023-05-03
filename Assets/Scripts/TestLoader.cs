using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for loading a saved test
/// </summary>
public class TestLoader : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The prefab of the selectable object")]
    private GameObject spheresPrefab;
    [SerializeField]
    [Tooltip("The parent object of the selectable objects in the test area")]
    private GameObject testObjectParent;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "RunTest") LoadTest();
    }

    /// <summary>
    /// Loads a saved test based on the file path stored in <see cref="TestDataStatic"/>
    /// </summary>
    public void LoadTest()
    {
        // Destroys all game objects in the test area
        foreach (Transform child in testObjectParent.transform)
        {
            Destroy(child.gameObject);
        }
        // Loads json data of the test
        SaveObjectsScript loadedData = new();
        loadedData.LoadFromJSON();

        TestDataStatic.currentTestFilePath = TestDataStatic.testFilePath;

        TestDataStatic.visionDetectionTime = loadedData.selectableObjectData.visionDetectionTime;

        // Instantiates a selectable object for each object in the loaded json data
        foreach (SelectableObjectDataClass dataClass in loadedData.selectableObjectData)
        {
            GameObject o = Instantiate(spheresPrefab, testObjectParent.transform);
            SelectableObject so = o.transform.Find("Sphere").GetComponent<SelectableObject>();

            // Sets the position of the prefab
            so.gameObject.transform.position = dataClass.startPosistion;
            o.transform.Find("Ghost Sphere").transform.position = dataClass.endPosistion;

            // Sets parameters of the selectable object to correct values
            so.startPos = dataClass.startPosistion;
            so.endPos = dataClass.endPosistion;
            so.scale = dataClass.scale;
            so.moveTime = dataClass.moveTime;
            so.startDelay = dataClass.startDelay;
            so.hasMovement = dataClass.hasMovement;
            so.loopMovement = dataClass.loopMovement;
            so.objectType = dataClass.objectType;
            if (so.objectType == "fullsphere") so.ChangeToFullSphere();
            else if (so.objectType == "righthalf") so.ChangeToRightHalf();
            else if (so.objectType == "lefthalf") so.ChangeToLeftHalf();
            so.transform.localScale = so.scale;

            so.ShowGhostSphere(so.hasMovement);
        }
    }

}
