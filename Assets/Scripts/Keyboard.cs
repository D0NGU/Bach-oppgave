using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public GameObject keyboard;
    public TMP_InputField inputField;

    public void SetInputField()
    {
        Debug.Log("String: " + inputField.text);
    }

    public void InsertChar(string c)
    {
        inputField.text += c;
    }

    public void DeleteChar()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }
}