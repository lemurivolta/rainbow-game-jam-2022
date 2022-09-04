using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player1KeyBindings", menuName = "Player Key Bindings", order = 1)]
public class PlayerKeyBindings : ScriptableObject
{
    public KeyCode Up;
    public KeyCode Right;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Action;
    public KeyCode Switch;
}
