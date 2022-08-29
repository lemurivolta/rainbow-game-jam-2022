using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorToDirectionFloat
{
    public static float ToDirectionFloat(this Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // x wins: set direction only based on x movement
            return direction.x > 0 ? 1.0f : 3.0f;
        }
        else
        {
            // y wins: set direction only based on y movement
            return direction.y > 0 ? 0.0f : 2.0f;
        }
    }
}
