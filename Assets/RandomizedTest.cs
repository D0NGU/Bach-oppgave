using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedTest : MonoBehaviour
{
    private TestArea testArea;
    private Coroutine coroutine;

    public GameObject spherePrefab;
    public GameObject testObjectParent;
    private bool showLeft = true;
    private bool showRight = true;
    private int percentageFullSpheres = 100;
    private int percentageLeftHalves = 0;

    public int spawnInterval = 10;
    public int spawnAmount = 1;

    private void Awake()
    {
        testArea = GameObject.Find("TestArea").GetComponent<TestArea>();
    }


    public void StartTest()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            ClearTestArea();

            return;
        }

        coroutine = StartCoroutine(RandomizedSpawn());
    }

    private IEnumerator RandomizedSpawn()
    {
        while (true) 
        {
            ClearTestArea();

            var dict = testArea.GetTestAreaBounds();

            int numberOfFullSpheres = (spawnAmount * percentageFullSpheres) / 100;

            int numberOfLeftHalves = ((spawnAmount - numberOfFullSpheres) * percentageLeftHalves) / 100;

            for (int i = 0; i < spawnAmount; i++)
            {
                GameObject gameObject = Instantiate(spherePrefab, testObjectParent.transform);


                if (i >= numberOfFullSpheres && i < (numberOfFullSpheres + numberOfLeftHalves)) 
                {
                    gameObject.transform.Find("Sphere").gameObject.GetComponent<SelectableObject>().ChangeToLeftHalf();
                }
                else if (i >= (numberOfFullSpheres + numberOfLeftHalves)) 
                {
                    gameObject.transform.Find("Sphere").gameObject.GetComponent<SelectableObject>().ChangeToRightHalf();
                }

                
                if (showLeft && !showRight)
                {
                    gameObject.transform.position = new Vector3(Random.Range(dict["minX"], 0.0f), Random.Range(dict["minY"], dict["maxY"]), Random.Range(dict["minZ"], dict["maxZ"]));
                }
                else if (!showLeft && showRight)
                {
                    gameObject.transform.position = new Vector3(Random.Range(0.0f, dict["maxX"]), Random.Range(dict["minY"], dict["maxY"]), Random.Range(dict["minZ"], dict["maxZ"]));
                }
                else
                {
                    gameObject.transform.position = new Vector3(Random.Range(dict["minX"], dict["maxX"]), Random.Range(dict["minY"], dict["maxY"]), Random.Range(dict["minZ"], dict["maxZ"]));
                }
            }

            yield return new WaitForSeconds(spawnInterval);

        }
    }

    private void ClearTestArea()
    {
        foreach (Transform child in testObjectParent.transform)
        {
            Destroy(child.gameObject);
        }
    }



    public void ShowLeftSide()
    {
        showLeft = true;
        showRight = false;
    }

    public void ShowRightSide()
    {
        showLeft = false;
        showRight = true;
    }

    public void ShowBothSides()
    {
        showLeft = true;
        showRight = true;
    }

    public void SetFullSpherePercentage(string text)
    {
        if (int.TryParse(text, out int result))
        {
            percentageFullSpheres = result;
        }
    }

    public void SetLeftHalvesPercentage(string text)
    {
        if (int.TryParse(text, out int result))
        {
            percentageLeftHalves = result;
        }
    }

    public void SetSpawnAmount(string text)
    {
        if (int.TryParse(text, out int result))
        {
            spawnAmount = result;
        }
    }

    public void SetSpawnInterval(string text)
    {
        if (int.TryParse(text, out int result))
        {
            spawnInterval = result;
        }
    }

}
