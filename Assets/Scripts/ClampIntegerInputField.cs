using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Responsible for clamping the value of a TMPro input field
/// </summary>
public class ClampIntegerInputField : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Minimum value of the input field")]
    private int min = 0;
    [SerializeField]
    [Tooltip("Maximum value of the input field")]
    private int max = 100;

    private int numberToClamp;
    private int result;

    private TMP_InputField TMP_IF;

    private void Awake()
    {
        TMP_IF = GetComponent<TMP_InputField>();
    }

    void Update()
    {
        if (int.TryParse(TMP_IF.text, out numberToClamp))
        {
            result = Mathf.Clamp(numberToClamp, min, max);
            TMP_IF.text = result.ToString();
        }
    }
}
