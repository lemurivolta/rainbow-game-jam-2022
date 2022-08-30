using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ScoutInputHandler : MonoBehaviour
{
    public InputActionAsset InputActionAsset;

    public UnityEvent<InputAction.CallbackContext> MovementP1;

    public UnityEvent<InputAction.CallbackContext> MovementP2;

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
    }

    // TODO: unregister
}
