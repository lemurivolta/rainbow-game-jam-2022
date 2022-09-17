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
        UpdateAndVisibility();
    }

    /// <summary>
    /// Event handler for when a character enters the trigger zone
    /// </summary>
    public void OnCharacterApproach()
    {
        NumNearbyCharacters += 1;
        UpdateAndVisibility();
    }

    /// <summary>
    /// Event handler for when a character exits the trigger zone
    /// </summary>
    public void OnCharacterDepart()
    {
        NumNearbyCharacters += -1;
        UpdateAndVisibility();
    }

    /// <summary>
    /// Set the visibility of the sprite renderer (if already available).
    /// </summary>
    private void UpdateAndVisibility()
    {
        if (SpriteRenderer != null)
        {
            SpriteRenderer.enabled = NumNearbyCharacters > 0;
        }
    }
}
