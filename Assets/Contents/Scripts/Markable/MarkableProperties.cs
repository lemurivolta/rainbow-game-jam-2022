using UnityEngine;

[CreateAssetMenu(fileName = "MarkableProperties", menuName = "rainbow-game-jam-2022/MarkableProperties", order = 0)]
public class MarkableProperties : ScriptableObject {
    [SerializeField] private Color _color;
    public Color Color => _color;

    [SerializeField] private float _k;
    public float K => _k;

    [SerializeField] private float _period;
    public float Period => _period;

    [SerializeField] private float _fadeIn = 0.5f;
    public float FadeIn => _fadeIn;
}