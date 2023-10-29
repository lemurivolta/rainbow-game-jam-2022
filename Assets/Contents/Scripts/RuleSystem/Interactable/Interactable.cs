using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    public void OnCharacterApproach(GameObject go)
    {
        OnCharacter(go, NearbyCharacters.Add, ApproachedByPlayer);
    }

    /// <summary>
    /// Event handler for when a character is no longer near enough.
    /// </summary>
    /// <param name="collision">The collider information</param>
    public void OnCharacterDepart(GameObject go)
    {
        OnCharacter(go, NearbyCharacters.Remove, LeftByPlayer);
    }

    private void OnCharacter(GameObject go, SetAction action, UnityEvent<CharacterInfo.Players> ev)
    {
        var ccb = go.transform
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
        if ((Balloon.Instance && Balloon.Instance.isBarking) || GameOverManagement.InGameOverSequence)
        {
            return; // don't do anything if the barks are on or in game over
        }
        foreach (var activePlayer in NearbyCharacters)
        {
            if (activePlayer.Player == player)
            {
                if (CameraStateUpdater != null && CameraStateUpdater.NumCameras > 0)
                {
                    // oops: object was under camera and we tried to interact!
                    SchermateManager.Instance.GameOver(
                        CameraStateUpdater.Cameras
                        .FirstOrDefault()?
                        .GetComponentInChildren<GameOverCauseSpriteRenderer>());
                }
                else
                {
                    // check if this character can interact with this interactable
                    if (!CanInteract(activePlayer))
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

    /// <summary>
    /// Check whether given character can interact with this interactable.
    /// </summary>
    /// <param name="activePlayer">The player to check.</param>
    /// <returns>Whether given player can interact with this interactable.</returns>
    public bool CanInteract(CharacterInfo activePlayer)
    {
        return !((!CanCloeInteract && activePlayer.Character == CHARACTER.CLOE) ||
            (!CanMarielleInteract && activePlayer.Character == CHARACTER.MARIELLE) ||
            (!CanSarahInteract && activePlayer.Character == CHARACTER.SARAH) ||
            (!CanYelenaInteract && activePlayer.Character == CHARACTER.YELENA));
    }
}
