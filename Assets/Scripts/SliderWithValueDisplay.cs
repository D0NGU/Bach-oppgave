using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SliderWithValueDisplay : MonoBehaviour
{
    public GameObject gameObjectToShow;
    public TMP_Text valueDisplayText;

    [SerializeField] private float hideDelay = 2.0f;
    private Coroutine coroutine;

    public void ShowElement()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;

            gameObjectToShow.SetActive(false);

        }

        valueDisplayText.GetComponent<TextMeshProUGUI>().text = Math.Round(GetComponent<Slider>().value, 2).ToString();

        coroutine = StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        gameObjectToShow.SetActive(true);

        yield return new WaitForSeconds(hideDelay);

        gameObjectToShow.SetActive(false);
    }

}
