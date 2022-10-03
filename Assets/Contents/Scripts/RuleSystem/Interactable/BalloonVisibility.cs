using UnityEngine;

[Tooltip("Component that manages the visibility of the balloon according to whether there are characters nearby")]
public class BalloonVisibility : MonoBehaviour
{
    /// <summary>
    /// Layer for characters.
    /// </summary>
    public LayerMask CharacterLayer;

    /// <summary>
    /// The current number of characters nearby.
    /// </summary>
    private int NumNearbyCharacters = 0;

    /// <summary>
    /// The sprite renderer for the balloon.
    /// </summary>
    private SpriteRenderer SpriteRenderer;

    public Interactable Interactable;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisibility();
    }

    /// <summary>
    /// Event handler for when a character enters the trigger zone
    /// </summary>
    public void OnCharacterApproach(GameObject go)
    {
        if (!IsCharacter(go) || !CanInteract(go))
        {
            return;
        }
        NumNearbyCharacters += 1;
        UpdateVisibility();
    }

    /// <summary>
    /// Event handler for when a character exits the trigger zone
    /// </summary>
    public void OnCharacterDepart(GameObject go)
    {
        if (!IsCharacter(go) || !CanInteract(go))
        {
            return;
        }
        NumNearbyCharacters += -1;
        UpdateVisibility();
    }

    private bool IsCharacter(GameObject go)
    {
        return (CharacterLayer & (1 << go.layer)) != 0;
    }

    private bool CanInteract(GameObject go)
    {
        var characterInfo = go.transform.parent.GetComponent<CharacterInfo>();
        var rv = Interactable.CanInteract(characterInfo);
        return rv;
    }

    /// <summary>
    /// Set the visibility of the sprite renderer (if already available).
    /// </summary>
    private void UpdateVisibility()
    {
        if (SpriteRenderer != null)
        {
            SpriteRenderer.enabled = NumNearbyCharacters > 0;
        }
    }
}
