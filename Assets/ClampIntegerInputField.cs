using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClampIntegerInputField : MonoBehaviour
{
    [SerializeField] private int min = 0;
    [SerializeField] private int max = 100;
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
