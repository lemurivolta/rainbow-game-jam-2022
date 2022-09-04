using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// All the players currently near enough to activate the interactable.
    /// </summary>
    private List<CharacterInfo> NearbyCharacters = new();

    private CameraStateUpdater CameraStateUpdater;

    private void Start()
    {
        CameraStateUpdater = GetComponentInChildren<CameraStateUpdater>();
    }

    /// <summary>
    /// Event handler for when a character gets near enough.
    /// </summary>
    /// <param name="collision">The collider information</param>
    public void OnCharacterApproach(Collider2D collision)
    {
        var ccb = collision.gameObject.GetComponent<CharacterInfo>();
        if (ccb != null && NearbyCharacters.IndexOf(ccb) < 0) {
            NearbyCharacters.Add(ccb);
        }
    }

    /// <summary>
    /// Event handler for when a character is no longer near enough.
    /// </summary>
    /// <param name="collision">The collider information</param>
    public void OnCharacterDepart(Collider2D collision)
    {
        var ccb = collision.gameObject.GetComponent<CharacterInfo>();
        if (ccb != null && NearbyCharacters.IndexOf(ccb) >= 0)
        {
            NearbyCharacters.Remove(ccb);
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

    /// <summary>
    /// Check if the given player has its active character near enough to activate
    /// the action.
    /// </summary>
    /// <param name="player">Player to check.</param>
    private void OnAction(CharacterInfo.Players player) {
        foreach(var activePlayer in NearbyCharacters)
        {
            if(activePlayer.Player == player)
            {
                if (CameraStateUpdater != null && CameraStateUpdater.NumCameras > 0)
                {
                    // oops: object was under camera and we tried to interact!
                    SchermateManager.Instance.Restart();
                }
                else
                {
                    // ok, we can do it!
                    Interaction.Invoke();
                }
                break;
            }
        }
    }
}
