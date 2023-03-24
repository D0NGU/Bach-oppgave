using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallButtonPanel : MonoBehaviour
{
    public GameObject saveTestOptionsView;
    public GameObject scrollView;
    

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainScreen");
    }

    public void SaveTestOptionsViewToggle()
    {
        saveTestOptionsView.SetActive(!saveTestOptionsView.activeSelf);
        scrollView.SetActive(false);
    }

    public void HideSaveTestOptionsView()
    {
        saveTestOptionsView.SetActive(false);
    }
}
