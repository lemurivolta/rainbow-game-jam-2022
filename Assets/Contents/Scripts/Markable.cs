using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Markable : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    [SerializeField] private Color _markColor = Color.red;

    [SerializeField] private float _k = 2;

    [SerializeField] private float _period = 1;

    private const string markColorProperty = "_MarkColor";
    private const string kProperty = "_K";
    private const string phaseProperty = "_Phase";

    private Material GetMaterial()
    {
        return _renderer.sharedMaterial;
    }

    private void Start()
    {
        Debug.Log($"default mark color is " + GetMaterial().GetColor(markColorProperty).ToString());
        GetMaterial().SetColor(markColorProperty, Color.white);
        GetMaterial().SetFloat(kProperty, _k);
        GetMaterial().SetFloat(phaseProperty, 0);
    }

    private Coroutine _phasingCoroutine = null;

    public void StartMark()
    {
        GetMaterial().SetColor(markColorProperty, _markColor);
        StopPhasing();
        StartCoroutine(Phase());
    }

    public void StopMark()
    {
        StopPhasing();
        GetMaterial().SetColor(markColorProperty, Color.white);
    }

    private void StopPhasing()
    {
        if(_phasingCoroutine != null)
        {
            StopCoroutine(_phasingCoroutine);
            _phasingCoroutine = null;
        }
    }

    private IEnumerator Phase()
    {
        for(; ; )
        {
            GetMaterial().SetFloat(phaseProperty, (Time.time % _period) / _period);
            yield return null;
        }
    }
}
