using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A component that invokes events when another game object is inside our trigger and is visible
/// to us (that is, there's no obstacle on the ray between us and the target).
/// The checks of visibility between objects are made between their transform's positions, but
/// if we find a child of the object with name SightCoordinatesReference, that transform's position
/// is used instead. It's usually useful to set SightCoordinatesReference at the center of the point where
/// it logically touches the ground (usually at its feet in the center of the collision box, if it's a
/// character).
/// </summary>
public class SightTrigger : MonoBehaviour
{
    /// <summary>
    /// Objects currently inside the trigger zone, both visible and invisible to us.
    /// </summary>
    private List<GameObject> gameObjectsInTriggerZone = new();

    /// <summary>
    /// Current visibility status of the objects in our trigger zone.
    /// </summary>
    private Dictionary<GameObject, bool> gameObjectVisibility = new Dictionary<GameObject, bool>();

    [Tooltip("Collision mask to catch all object that can obstruct the view between this object and its targets")]
    public LayerMask ObstructionLayerMask = 0;

    [Tooltip("Event invoked whenever another object gets visible to us.")]
    public UnityEvent<GameObject> GotVisible;

    [Tooltip("Event invoked whenever another object gets hidden to us.")]
    public UnityEvent<GameObject> GotHidden;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // add the object to the ones to consider during Update and mark it as hidden
        // (right now it is supposed to be hidden, maybe during next Update we will discover
        // that it's visible and invoke the event)
        gameObjectsInTriggerZone.Add(other.gameObject);
        gameObjectVisibility[other.gameObject] = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // it's surely invisible now
        CheckVisibilityChanged(other.gameObject, false);
        // removes the object from our data structures
        gameObjectsInTriggerZone.Remove(other.gameObject);
        gameObjectVisibility.Remove(other.gameObject);
    }

    /// <summary>
    /// Map between game objects and transform (position) of the reference point for the sight
    /// checks. It's a weak hash map, so that when game objects get GC'd, they disappeare from
    /// here too.
    /// </summary>
    private static ConditionalWeakTable<GameObject, Transform> SightCoordinatesReferences = new();

    /// <summary>
    /// Get the position of the game object possibly using the sight coordinates reference,
    /// if present.
    /// </summary>
    /// <param name="o">The game object to get the coordinates of.</param>
    /// <returns>the coordinates of the gameobject.</returns>
    private Vector3 GetPosition(GameObject o)
    {
        // look for the sight coordinates reference
        var transform = SightCoordinatesReferences.GetValue(
            o,
            _ =>
            {
                var result = o.transform.Find("SightCoordinatesReference");
                if (result == null)
                {
                    result = o.transform;
                }
                // Debug.Log($"SightCoordinatesReference of {o.name} is {(result == null ? "<not found>" : result.name)}");
                return result;
            }
        );
        // return the coordinates of either the reference point, or the transform point
        return (transform == null ? o.transform : transform).position;
    }

    private void Update()
    {
        // check all objects inside our trigger zone
        foreach (var go in gameObjectsInTriggerZone)
        {
            if(go != null)
            {
                // send a ray between us that interacts with objects obstructing our view
                var myPosition = GetPosition(this.gameObject);
                var otherPosition = GetPosition(go);
                var queriesHitTriggers = Physics2D.queriesHitTriggers;
                Physics2D.queriesHitTriggers = false;
                var hit = Physics2D.Raycast(
                    myPosition,
                    otherPosition - myPosition,
                    (otherPosition - myPosition).magnitude,
                    ObstructionLayerMask
                );
                Physics2D.queriesHitTriggers = queriesHitTriggers;
                // the other object is visible if there are NO objects obstructing our view
                var visible = hit.collider == null;
                // if (hit.collider != null)
                // {
                //     Debug.Log($"Cannot see {go.name} because it is obstructed by {hit.collider.gameObject.name}", hit.collider.gameObject);
                // }
                // check if we must send events
                CheckVisibilityChanged(go, visible);
            }            
        }
    }

    /// <summary>
    /// Check if a GotVisible or GotHidden event must be sent. This happens only if the visibility
    /// status changed from last time.
    /// </summary>
    /// <param name="gameObject">The object to check.</param>
    /// <param name="visible">The object visibility status.</param>
    private void CheckVisibilityChanged(GameObject gameObject, bool visible)
    {
        if (gameObjectVisibility[gameObject] != visible)
        {
            // Debug.Log($"game object {this.gameObject.name} {(visible ? "sees" : "no longer sees")} {gameObject.name}");
            gameObjectVisibility[gameObject] = visible;
            (visible ? GotVisible : GotHidden).Invoke(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        var myPosition = GetPosition(this.gameObject);
        foreach (var gameObject in gameObjectsInTriggerZone)
        {
            Gizmos.color = gameObjectVisibility[gameObject] ?
                Color.magenta :
                Color.gray;
            Gizmos.DrawLine(myPosition, GetPosition(gameObject));
        }
    }
}
