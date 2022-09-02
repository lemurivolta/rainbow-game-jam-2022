using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterInputHandler : MonoBehaviour
{
    public InputActionAsset InputActionAsset;

    public UnityEvent<InputAction.CallbackContext> MovementP1;

    public UnityEvent<InputAction.CallbackContext> MovementP2;

    public UnityEvent<InputAction.CallbackContext> SwitchCharactersP1;

    public UnityEvent<InputAction.CallbackContext> SwitchCharactersP2;

    public UnityEvent<InputAction.CallbackContext> ReleaseSwitchCharactersP1;

    public UnityEvent<InputAction.CallbackContext> ReleaseSwitchCharactersP2;

    // Start is called before the first frame update
    void Start()
    {
        InputActionAsset.Enable();

        var movementP1 = InputActionAsset.FindAction("MovementP1");
        movementP1.started += MovementP1.Invoke;
        movementP1.performed += MovementP1.Invoke;
        movementP1.canceled += MovementP1.Invoke;

        var movementP2 = InputActionAsset.FindAction("MovementP2");
        movementP2.started += MovementP2.Invoke;
        movementP2.performed += MovementP2.Invoke;
        movementP2.canceled += MovementP2.Invoke;

        InputActionAsset.FindAction("SwitchCharactersP1").performed +=
            SwitchCharactersP1.Invoke;

        InputActionAsset.FindAction("SwitchCharactersP2").performed +=
            SwitchCharactersP2.Invoke;

        InputActionAsset.FindAction("ReleaseSwitchCharactersP1").performed +=
            ReleaseSwitchCharactersP1.Invoke;

        InputActionAsset.FindAction("ReleaseSwitchCharactersP2").performed +=
            ReleaseSwitchCharactersP2.Invoke;
    }

    // TODO: unregister
}
