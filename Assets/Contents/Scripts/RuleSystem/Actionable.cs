using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component used by the RuleSystem to make objects perform actions.
/// </summary>
public class Actionable : MonoBehaviour
{
    /// <summary>
    /// Event raised when a character activates the actionable.
    /// </summary>
    public UnityEvent ActedUpon;

    /// <summary>
    /// Event raised when a character approaches something that controls this actionable.
    /// </summary>
    public UnityEvent<CharacterInfo.Players> PlayerApproachedControl;

    /// <summary>
    /// Event raised when a character leaves something that control this actionable.
    /// </summary>
    public UnityEvent<CharacterInfo.Players> PlayerLeftControl;

    /// <summary>
    /// Invoke the ActedUpon event.
    /// </summary>
    public void PerformAction()
    {
        ActedUpon.Invoke();
    }

    /// <summary>
    /// Invoke the PlayerApproachedControl event.
    /// </summary>
    public void PlayerJustApproachedControl(CharacterInfo.Players player)
    {
        PlayerApproachedControl.Invoke(player);
    }

    /// <summary>
    /// Invoke the PlayerLeftControl event.
    /// </summary>
    public void PlayerJustLeftControl(CharacterInfo.Players player)
    {
        PlayerLeftControl.Invoke(player);
    }
}
