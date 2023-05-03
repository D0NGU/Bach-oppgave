using UnityEngine;
using TMPro;

/// <summary>
/// Controls a UI Keyboard
/// </summary>
public class Keyboard : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The TMPro input field of the keyboard")]
    private TMP_InputField inputField;

    /// <summary>
    /// Inserts a character into the keyboards input field
    /// </summary>
    /// <param name="c">Character to instert</param>
    public void InsertChar(string c)
    {
        inputField.text += c;
    }

    /// <summary>
    /// Removes the last character in the input field
    /// </summary>
    public void DeleteChar()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }
}