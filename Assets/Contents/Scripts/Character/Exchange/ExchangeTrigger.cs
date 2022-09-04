using UnityEngine;
using UnityAtoms.BaseAtoms;

/// <summary>
/// A component that handles the trigger of the ExchangeEnabled event, turning it on when
/// main characters are near enough and in sight of each other.
/// </summary>
public class ExchangeTrigger : MonoBehaviour
{
    [Tooltip("The event to raise when the exchange trigger must get enabled or disabled")]
    public BoolVariable ExchangeEnabled;

    /// <summary>
    /// Transform of the target, if we got one
    /// </summary>
    private Transform target = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // when the trigger turns on, just save the other target so to check possible obstacles
        // on Update
        target = collision.gameObject.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // when we exit trigger, forget our target and disable exchange
        target = null;
        ExchangeEnabled.Value = false;
    }

    [Tooltip("Layer mask for objects that can block the exchange when between characters")]
    public LayerMask BlockingMask;

    private void Update()
    {
        // only check exchange status if we have a target inside the collider
        if (target != null)
        {
            // in that case, value is on only if there's no collider in between
            var collider = Physics2D.Raycast(
                transform.position,
                target.position - transform.position,
                (target.position - transform.position).magnitude, BlockingMask
            ).collider;
            ExchangeEnabled.Value = collider == null;
        }
    }
}
