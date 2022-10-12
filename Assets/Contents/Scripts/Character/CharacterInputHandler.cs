using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterInputHandler : MonoBehaviour
{
    public PlayerKeyBindings Player1KeyBindings;
    public PlayerKeyBindings Player2KeyBindings;
    public float LongPressMinimumDuration = 0.3f;

    public InputActionAsset InputActionAsset;

    public UnityEvent<Vector2> MovementP1;

    public UnityEvent<Vector2> MovementP2;

    public UnityEvent SwitchCharactersP1;

    public UnityEvent SwitchCharactersP2;

    public UnityEvent ExchangeCharactersStartedP1;
    public UnityEvent ExchangeCharactersStartedP2;
    public UnityEvent ExchangeCharactersStoppedP1;
    public UnityEvent ExchangeCharactersStoppedP2;
    public UnityEvent ActionP1;

    public UnityEvent ActionP2;

    private Vector2 LastMovementP1 = Vector2.zero;
    private Vector2 LastMovementP2 = Vector2.zero;

    private Coroutine ExchangeTimerP1Coroutine = null;
    private bool SentExchangeP1 = false;
    private Coroutine ExchangeTimerP2Coroutine = null;
    private bool SentExchangeP2 = false;

    private void Update()
    {
        if (!Player1KeyBindings || !Player2KeyBindings)
        {
            Debug.LogWarning("No key bindings set.", this);
            return;
        }
        HandleInput(Player1KeyBindings, MovementP1, LastMovementP1, SwitchCharactersP1, ExchangeCharactersStartedP1, ExchangeCharactersStoppedP1, ActionP1, out LastMovementP1, ref ExchangeTimerP1Coroutine, (bool value) => { SentExchangeP1 = value; }, () => SentExchangeP1);
        HandleInput(Player2KeyBindings, MovementP2, LastMovementP2, SwitchCharactersP2, ExchangeCharactersStartedP2, ExchangeCharactersStoppedP2, ActionP2, out LastMovementP2, ref ExchangeTimerP2Coroutine, (bool value) => { SentExchangeP2 = value; }, () => SentExchangeP2);
    }

    private void HandleInput(
        PlayerKeyBindings bindings,
        UnityEvent<Vector2> Movement,
        Vector2 LastMovement,
        UnityEvent SwitchCharacters,
        UnityEvent ExchangeCharactersStarted,
        UnityEvent ExchangeCharactersStopped,
        UnityEvent Action,
        out Vector2 lastMovementOut,
        ref Coroutine exchangeTimerCoroutine,
        Action<bool> setSentExchange,
        Func<bool> getSentExchange)
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey(bindings.Up))
        {
            movement.y = 1;
        }
        if (Input.GetKey(bindings.Down))
        {
            movement.y = -1;
        }
        if (Input.GetKey(bindings.Left))
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

        if (Input.GetKeyDown(bindings.Switch))
        {
            // pressed the switch button: wait for a small while to check if it's a switch or exchange
            exchangeTimerCoroutine = StartCoroutine(ExchangeTimerCoroutine(setSentExchange, ExchangeCharactersStarted));
        }
        if (Input.GetKeyUp(bindings.Switch))
        {
            // check if we switched (button released before timeout) or if we stopped an exchange (button release after timeout)
            if (getSentExchange())
            {
                setSentExchange(false);
                ExchangeCharactersStopped.Invoke();
            }
            else
            {
                StopCoroutine(exchangeTimerCoroutine);
                exchangeTimerCoroutine = null;
                SwitchCharacters.Invoke();
            }
        }

        if (Input.GetKeyDown(bindings.Action))
        {
            Action.Invoke();
        }

        lastMovementOut = movement;
    }

    private IEnumerator ExchangeTimerCoroutine(Action<bool> setSetExchange, UnityEvent ExchangeCharactersStarted)
    {
        setSetExchange(false);
        yield return new WaitForSeconds(LongPressMinimumDuration);
        ExchangeCharactersStarted.Invoke();
        setSetExchange(true);
    }
}
