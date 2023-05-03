using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the main UI wrist menu with general options
/// </summary>
public class WristUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Input Action Asset")]
    private InputActionAsset inputActions;
    [SerializeField]
    [Tooltip("The XR Origin of the player")]
    private GameObject xrOrigin;

    private Canvas wristUI;
    private InputAction menuButton;
    
    void Start()
    {
        wristUI = GetComponent<Canvas>();
        menuButton = inputActions.FindActionMap("XRI LeftHand Interaction").FindAction("Menu");
        menuButton.Enable();
        menuButton.performed += ToggleMenu;
    }

    private void OnDestroy()
    {
        menuButton.performed -= ToggleMenu;
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        wristUI.enabled = !wristUI.enabled;
    }

    public void ResetPlayerPosition()
    {
        xrOrigin.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

}
