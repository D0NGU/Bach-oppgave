using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViveSR.anipal.Eye;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the scroll views used for loading tests
/// </summary>
public class ScrollViewController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Prefab of the button used to fill the scroll view")]
    private GameObject buttonPrefab;
    [SerializeField]
    [Tooltip("Parent game object of the buttons in the scroll view")]
    private Transform newParent;
    [Tooltip("Scrollview game object")]
    [SerializeField]
    private GameObject scrollView;
    [SerializeField]
    [Tooltip("Parent game object of the scroll view control buttons")]
    private GameObject scrollViewButtons;

    private List<GameObject> testButtonList = new();

    private void Update()
    {
        // Hides the scroll view buttons when the scroll view is inactive
        if (!scrollView.activeSelf)
        {
            scrollViewButtons.SetActive(false);
        }
    }

    /// <summary>
    /// Fills the scroll view with a button for each of the saved tests
    /// </summary>
    public void FillScrollView()
    {
        scrollView.SetActive(!scrollView.activeSelf);

        testButtonList.Clear();
        foreach (Transform child in newParent.transform)
        {
            Destroy(child.gameObject);
        }

        if (scrollView.activeSelf)
        {
            DirectoryInfo dir = new DirectoryInfo(TestDataStatic.testFolderPath);
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

    /// <summary>
    /// Runs when clicking on a button in the scroll view
    /// Sets the test file path and shows the scroll view buttons
    /// </summary>
    /// <param name="path">The path of the selected test file</param>
    private void OnTestButtonClick(string path)
    {
        TestDataStatic.testFilePath = path;
        scrollViewButtons.SetActive(true);
        
    }
}
