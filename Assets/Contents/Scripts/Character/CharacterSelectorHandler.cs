using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectorHandler : MonoBehaviour
{
    public GameObject Selector;

    public void ShowSelector(bool show)
    {
        Selector.SetActive(show);
    }
}
