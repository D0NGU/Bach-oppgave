using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Controls a UI slider with a value display
/// </summary>
public class SliderWithValueDisplay : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The UI panel to show when changing the value of the slider")]
    private GameObject gameObjectToShow;
    [SerializeField]
    [Tooltip("TMPro text that displays the value of the slider")]
    private TMP_Text valueDisplayText;

    [SerializeField]
    [Tooltip("Delay in seconds to wait before hiding the value display after changing the value of the slider")]
    private float hideDelay = 2.0f;
    private Coroutine coroutine;

    /// <summary>
    /// Updates the value of the slider, shows the value display
    /// and starts the timer for hiding the value display again.
    /// </summary>
    public void ShowElement()
    {
        // Hides the value display if it is already visible
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;

            gameObjectToShow.SetActive(false);

        }

        valueDisplayText.GetComponent<TextMeshProUGUI>().text = Math.Round(GetComponent<Slider>().value, 2).ToString();

        coroutine = StartCoroutine(Timer());
    }

    /// <summary>
    /// Waits for seconds equal to hideDelay before hiding the value display
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator Timer()
    {
        gameObjectToShow.SetActive(true);

        yield return new WaitForSeconds(hideDelay);

        gameObjectToShow.SetActive(false);
    }

}
