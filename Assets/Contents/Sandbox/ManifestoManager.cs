using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManifestoManager : MonoBehaviour
{
    public List<Bark> Barks;

    private void Start()
    {
        Balloon.Instance.PlayBark(Barks[0]);
    }
}
