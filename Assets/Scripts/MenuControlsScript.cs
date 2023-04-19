using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class MenuControlsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private Transform newParent;
    [SerializeField]
    private GameObject scrollView;
    [SerializeField]
    private GameObject scrollViewButtons;

    private List<GameObject> testButtonList = new();


    public void FillScrollView()
    {
        scrollView.SetActive(!scrollView.activeSelf);

        testButtonList.Clear();
        foreach (Transform child in newParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (scrollView.activeSelf)
        {
            string path = Application.dataPath + "/TestFiles";
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] info = dir.GetFiles("*.json");

            foreach (FileInfo f in info)
            {
                GameObject GObutton = Instantiate(buttonPrefab, newParent);
                GObutton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = f.Name.Remove(f.Name.Length - 5);
                GObutton.GetComponent<Button>().onClick.AddListener(() => OnTestButtonClick(f.FullName));
                testButtonList.Add(GObutton);

            }
        }
       
    }

    private void OnTestButtonClick(string path)
    {
        TestDataStatic.testFilePath = path;
        scrollViewButtons.SetActive(true);
        
    }

    public void ToggleActive(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

}
