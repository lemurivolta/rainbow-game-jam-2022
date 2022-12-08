using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Markable : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    [SerializeField] private MarkableProperties _markableProperties;

    private const string markColorProperty = "_MarkColor";
    private const string kProperty = "_K";
    private const string phaseProperty = "_Phase";

    private static Color transparent = new Color(0, 0, 0, 0);

    private Material GetMaterial()
    {
        return _renderer.material;
    }

    private void Start()
    {
        GetMaterial().SetColor(markColorProperty, transparent);
        GetMaterial().SetFloat(kProperty, _markableProperties.K);
        GetMaterial().SetFloat(phaseProperty, 0);
    }

    private Coroutine _phasingCoroutine = null;

    public void StartMark()
    {
        GetMaterial().SetColor(markColorProperty, _markableProperties.Color);
        StopPhasing();
        _phasingCoroutine = StartCoroutine(Phase());
    }

    public void StopMark()
    {
        StopPhasing();
        GetMaterial().SetColor(markColorProperty, transparent);
    }

    private void StopPhasing()
    {
        if (_phasingCoroutine != null)
        {
            StopCoroutine(_phasingCoroutine);
            _phasingCoroutine = null;
        }
    }

    private IEnumerator Phase()
    {
        var start = Time.time;
        for (; ; )
        {
            var t = (Time.time - start) / _markableProperties.FadeIn;
            GetMaterial().SetColor(markColorProperty,
                t < 1 ? Color.Lerp(transparent, _markableProperties.Color, t) : _markableProperties.Color
            );
            GetMaterial().SetFloat(kProperty, _markableProperties.K);
            GetMaterial().SetFloat(
                phaseProperty,
                ((Time.time % _markableProperties.Period) / _markableProperties.Period) * 2 * MathF.PI
            );
            yield return null;
        }
    }
}
