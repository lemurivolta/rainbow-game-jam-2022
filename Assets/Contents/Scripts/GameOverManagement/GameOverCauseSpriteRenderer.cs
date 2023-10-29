using JetBrains.Annotations;

using UnityEngine;

public class GameOverCauseSpriteRenderer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer => spriteRenderer;

    //// Show the Sprite triangles
    //void OnDrawGizmos()
    //{
    //    Sprite sprite = SpriteRenderer.sprite;

    //    ushort[] triangles = sprite.triangles;
    //    Vector2[] vertices = sprite.vertices;
    //    int a, b, c;

    //    Vector3 To(Vector2 from)
    //    {
    //        return transform.TransformPoint(from);
    //    }

    //    // draw the triangles using grabbed vertices
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < triangles.Length; i += 3)
    //    {
    //        a = triangles[i];
    //        b = triangles[i + 1];
    //        c = triangles[i + 2];

    //        var va = To(vertices[a]);
    //        var vb = To(vertices[b]);
    //        var vc = To(vertices[c]);

    //        //To see these you must view the game in the Scene tab while in Play mode
    //        Gizmos.DrawLine(va, vb);
    //        Gizmos.DrawLine(vb, vc);
    //        Gizmos.DrawLine(vc, va);
    //    }

    //    float minx = float.MaxValue, miny = float.MaxValue, maxx = float.MinValue, maxy = float.MinValue;
    //    foreach (var v in vertices)
    //    {
    //        minx = Mathf.Min(minx, v.x);
    //        miny = Mathf.Min(miny, v.y);
    //        maxx = Mathf.Max(maxx, v.x);
    //        maxy = Mathf.Max(maxy, v.y);
    //    }
    //    Gizmos.color = Color.blue;
    //    var p1 = new Vector2(minx + 0.1f, miny + 0.1f);
    //    var p2 = new Vector2(maxx - 0.1f, miny + 0.1f);
    //    var p3 = new Vector2(maxx - 0.1f, maxy - 0.1f);
    //    var p4 = new Vector2(minx + 0.1f, maxy - 0.1f);
    //    Gizmos.DrawLine(To(p1), To(p2));
    //    Gizmos.DrawLine(To(p2), To(p3));
    //    Gizmos.DrawLine(To(p3), To(p4));
    //    Gizmos.DrawLine(To(p4), To(p1));
    //}

    /// <summary>
    /// Get a rect of world coordinates that bound this sprite
    /// </summary>
    /// <returns></returns>
    public Rect GetBoundaryWorldRect()
    {
        var sprite = SpriteRenderer.sprite;
        var vertices = sprite.vertices;
        float minx = float.MaxValue, miny = float.MaxValue, maxx = float.MinValue, maxy = float.MinValue;
        foreach (var v in vertices)
        {
            minx = Mathf.Min(minx, v.x);
            miny = Mathf.Min(miny, v.y);
            maxx = Mathf.Max(maxx, v.x);
            maxy = Mathf.Max(maxy, v.y);
        }
        var minPoint = transform.TransformPoint(new Vector2(minx, miny));
        var maxPoint = transform.TransformPoint(new Vector2(maxx, maxy));
        return new Rect(minPoint.x, minPoint.y, maxPoint.x - minPoint.x, maxPoint.y - minPoint.y);
    }
}
