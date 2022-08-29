using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorToDirectionFloat
{
    public static float ToDirectionFloat(this Vector2 direction)
    {
        return direction.x > 0 ? 1.0f :
                direction.y < 0 ? 2.0f :
                direction.x < 0 ? 3.0f :
                0.0f;
    }
}
