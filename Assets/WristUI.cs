using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristUI : MonoBehaviour
{
    public InputActionAsset inputActions;

    private Canvas wristUI;
    private InputAction menuButton;

    // Start is called before the first frame update
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
}
