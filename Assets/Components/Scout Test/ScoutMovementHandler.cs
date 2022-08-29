using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoutMovementHandler : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        rigidBody2D.velocity = Speed * direction;
    }
}
