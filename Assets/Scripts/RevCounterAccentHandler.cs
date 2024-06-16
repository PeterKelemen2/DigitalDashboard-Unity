using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevCounterAccentHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component attached to the same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Check if the SpriteRenderer component is found
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the GameObject.");
            return;
        }

        // Set initial transparency (0 is fully transparent, 1 is fully opaque)
        // SetTransparency(0.5f); // 50% transparency
    }

    void Update()
    {
    }

    public void SetTransparency(float alpha)
    {
        if (spriteRenderer != null)
        {
            // Ensure alpha is clamped between 0 and 1
            alpha = Mathf.Clamp01(alpha);

            // Get the current color of the sprite
            Color color = spriteRenderer.color;

            // Set the alpha value
            color.a = alpha;

            // Apply the new color to the sprite
            spriteRenderer.color = color;
        }
    }

    public bool VibeCheck()
    {
        if (spriteRenderer)
        {
            return true;
        }
        return false;
    }
}