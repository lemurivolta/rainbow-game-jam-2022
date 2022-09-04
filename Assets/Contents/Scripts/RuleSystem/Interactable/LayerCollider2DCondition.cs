using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityAtoms;

[EditorIcon("atom-icon-sand")]
[CreateAssetMenu(menuName = "Unity Atoms/Conditions/Collider2D/Layer Condition", fileName = "LayerCollider2DCondition")]
public class LayerCollider2DCondition : Collider2DCondition
{
    public LayerMask AcceptableLayers;

    public override bool Call(Collider2D t)
    {
        return (AcceptableLayers & (1 << t.gameObject.layer)) != 0;
    }
}
