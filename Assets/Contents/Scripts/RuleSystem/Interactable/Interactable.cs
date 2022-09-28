using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// All the players currently near enough to activate the interactable.
    /// </summary>
    private HashSet<CharacterInfo> NearbyCharacters = new();

    private CameraStateUpdater CameraStateUpdater;

    public bool CanCloeInteract = true;

    public bool CanYelenaInteract = true;

    public bool CanSarahInteract = true;

    public bool CanMarielleInteract = true;

    private void Start()
    {
        CameraStateUpdater = GetComponentInChildren<CameraStateUpdater>();
    }

    private delegate bool SetAction(CharacterInfo characterInfo);

    /// <summary>
    /// Event handler for when a character gets near enough.
    /// </summary>
    /// <param name="collision">The collider information</param>
    public void OnCharacterApproach(Collider2D collision)
    {
        OnCharacter(collision, NearbyCharacters.Add, ApproachedByPlayer);
    }

    /// <summary>
    /// Event handler for when a character is no longer near enough.
    /// </summary>
    /// <param name="collision">The collider information</param>
    public void OnCharacterDepart(Collider2D collision)
    {
        OnCharacter(collision, NearbyCharacters.Remove, LeftByPlayer);
    }

    private void OnCharacter(Collider2D collision, SetAction action, UnityEvent<CharacterInfo.Players> ev)
    {
        var ccb = collision.gameObject.transform
            .parent.gameObject.GetComponent<CharacterInfo>();
        if (ccb != null)
        {
            action(ccb);
            ev.Invoke(ccb.Player);
        }
    }

    /// <summary>
    /// Event handler for when a player 1 uses the action command.
    /// </summary>
    public void OnActionP1()
    {
        OnAction(CharacterInfo.Players.P1);
    }

    /// <summary>
    /// Event handler for when a player 1 uses the action command.
    /// </summary>
    public void OnActionP2()
    {
        OnAction(CharacterInfo.Players.P2);
    }

    [Tooltip("Event raised when a player successfully interacts with this object")]
    public UnityEvent Interaction;

    [Tooltip("Event raised when a player approaches this object")]
    public UnityEvent<CharacterInfo.Players> ApproachedByPlayer;

    [Tooltip("Event raised when a player leaves this object")]
    public UnityEvent<CharacterInfo.Players> LeftByPlayer;

    /// <summary>
    /// Check if the given player has its active character near enough to activate
    /// the action.
    /// </summary>
    /// <param name="player">Player to check.</param>
    private void OnAction(CharacterInfo.Players player)
    {
        if (Balloon.Instance.isBarking)
        {
            return; // don't do anything if the barks are on
        }
        foreach (var activePlayer in NearbyCharacters)
        {
            if (activePlayer.Player == player)
            {
                if (CameraStateUpdater != null && CameraStateUpdater.NumCameras > 0)
                {
                    // oops: object was under camera and we tried to interact!
                    SchermateManager.Instance.GameOver();
                }
                else
                {
                    // check if this character can interact with this interactable
                    if ((!CanCloeInteract && activePlayer.Character == CHARACTER.CLOE) ||
                    (!CanMarielleInteract && activePlayer.Character == CHARACTER.MARIELLE) ||
                    (!CanSarahInteract && activePlayer.Character == CHARACTER.SARAH) ||
                    (!CanYelenaInteract && activePlayer.Character == CHARACTER.YELENA))
                    {
                        activePlayer
                            .gameObject
                            .GetComponent<UnknownAction>()
                            .ShowUnknownAction();
                        return;
                    }
                    // ok, we can do it!
                    Interaction.Invoke();
                    // also make the animation start
                    activePlayer.gameObject
                        .GetComponent<CharacterAnimationsHandler>()
                        .OnAction();
                }
                break;
            }
        }
    }
}
