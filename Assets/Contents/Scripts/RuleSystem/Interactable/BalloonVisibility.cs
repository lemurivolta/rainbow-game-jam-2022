using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("Component that manages the visibility of the balloon according to whether there are characters nearby")]
public class BalloonVisibility : MonoBehaviour
{
    /// <summary>
    /// The current number of characters nearby.
    /// </summary>
    private int NumNearbyCharacters = 0;

    /// <summary>
    /// The sprite renderer for the balloon.
    /// </summary>
    private SpriteRenderer SpriteRenderer;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Event handler for when a character enters the trigger zone
    /// </summary>
    public void OnCharacterApproach()
    {
        UpdateNearbyCharactersAndVisibility(1);
    }

    /// <summary>
    /// Event handler for when a character exits the trigger zone
    /// </summary>
    public void OnCharacterDepart()
    {
        UpdateNearbyCharactersAndVisibility(-1);
    }

    /// <summary>
    /// Updates the number of nearby characters and then re-compute the visibility.
    /// </summary>
    /// <param name="delta">The change in the number of nearby characters.</param>
    private void UpdateNearbyCharactersAndVisibility(int delta)
    {
        NumNearbyCharacters += delta;
        SpriteRenderer.enabled = NumNearbyCharacters > 0;
    }
}
