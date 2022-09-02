using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// A component to keep track of the companion of this character.
/// The companion is the other character controlled by the same player.
/// </summary>
public class CharacterControlledBy : MonoBehaviour
{
    /// <summary>
    /// All the players.
    /// </summary>
    [Serializable]
    public enum Players
    {
        /// <summary>
        /// First player.
        /// </summary>
        P1,
        /// <summary>
        /// Second player.
        /// </summary>
        P2
    }

    /// <summary>
    /// The player which controls this character.
    /// </summary>
    public Players Player;

    /// <summary>
    /// Cached companion.
    /// </summary>
    private CharacterControlledBy _Companion = null;

    /// <summary>
    /// All the (enabled) characters.
    /// </summary>
    private static readonly List<CharacterControlledBy> _AllObjects = new();

    private SimpleStateMachine.StateMachineRunner StateMachineRunner;

    private void Start()
    {
        StateMachineRunner = GetComponent<SimpleStateMachine.StateMachineRunner>();
        PlayerChanged.Invoke(Player);
    }

    public bool IsFollower { get { return StateMachineRunner.CurrentStateName == "Following";  } }

    private void OnEnable()
    {
        // when enabled, add to the list
        Debug.Assert(_AllObjects.IndexOf(this) == -1);
        _AllObjects.Add(this);
    }

    private void OnDisable()
    {
        // when disabled, remove from the list
        Debug.Assert(_AllObjects.IndexOf(this) >= 0);
        _AllObjects.Remove(this);
    }

    /// <summary>
    /// All the enabled CharacterControlledBy. Do not change this list.
    /// </summary>
    public static List<CharacterControlledBy> AllCharacterControlledBy
    {
        get
        {
            return _AllObjects;
        }
    }

    /// <summary>
    /// Check whether another character is my companion.
    /// </summary>
    /// <param name="c">The other character</param>
    /// <returns><c>true</c> if it's my companion, <c>false</c> otherwise.</returns>
    private bool IsMyCompanion(CharacterControlledBy c)
    {
        return c != null && c.Player == Player && c != this;
    }

    /// <summary>
    /// The other character controlled by the same player.
    /// </summary>
    public GameObject Companion
    {
        get
        {
            try
            {
                _Companion = IsMyCompanion(_Companion) ? _Companion : _AllObjects.First(IsMyCompanion);
                return _Companion.gameObject;
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Cannot find companion of " + gameObject.name);
            }
        }
    }

    public UnityEvent<Players> PlayerChanged;

    public void OnExchangePerformed()
    {
        if(!IsFollower)
        {
            Player = Player == Players.P1 ? Players.P2 : Players.P1;
            PlayerChanged.Invoke(Player);
        }
    }
}
