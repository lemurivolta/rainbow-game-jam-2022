using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterInputHandler : MonoBehaviour
{
    public PlayerKeyBindings Player1KeyBindings;
    public PlayerKeyBindings Player2KeyBindings;

    public InputActionAsset InputActionAsset;

    public UnityEvent<Vector2> MovementP1;

    public UnityEvent<Vector2> MovementP2;

    public UnityEvent SwitchCharactersP1;

    public UnityEvent SwitchCharactersP2;

    public UnityEvent ReleaseSwitchCharactersP1;

    public UnityEvent ReleaseSwitchCharactersP2;

    public UnityEvent ActionP1;

    public UnityEvent ActionP2;

    private Vector2 LastMovementP1 = Vector2.zero;
    private Vector2 LastMovementP2 = Vector2.zero;

    private void Update()
    {
        if(!Player1KeyBindings || !Player2KeyBindings)
        {
            Debug.LogWarning("No key bindings set.", this);
            return;
        }
        LastMovementP1 = HandleInput(Player1KeyBindings, MovementP1, LastMovementP1, SwitchCharactersP1, ReleaseSwitchCharactersP1, ActionP1);
        LastMovementP2 = HandleInput(Player2KeyBindings, MovementP2, LastMovementP2, SwitchCharactersP2, ReleaseSwitchCharactersP2, ActionP2);
    }

    private Vector2 HandleInput(
        PlayerKeyBindings bindings,
        UnityEvent<Vector2> Movement,
        Vector2 LastMovement,
        UnityEvent SwitchCharacters,
        UnityEvent ReleaseSwitchCharacters,
        UnityEvent Action)
    {
        Vector2 movement = Vector2.zero;
        if(Input.GetKey(bindings.Up)) {
            movement.y = 1;
        }
        if(Input.GetKey(bindings.Down)) {
            movement.y = -1;
        }
        if(Input.GetKey(bindings.Left))
        {
            movement.x = -1;
        }
        if (Input.GetKey(bindings.Right))
        {
            movement.x = 1;
        }
        movement.Normalize();
        if (movement != LastMovement)
        {
            Movement.Invoke(movement);
        }

        if(Input.GetKeyDown(bindings.Switch))
        {
            SwitchCharacters.Invoke();
        }
        if(Input.GetKeyUp(bindings.Switch))
        {
            ReleaseSwitchCharacters.Invoke();
        }

        if(Input.GetKey(bindings.Action))
        {
            Action.Invoke();
        }

        return movement;
    }
}
