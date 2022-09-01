using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitchHandler : MonoBehaviour
{
    private CharacterControlledBy characterControlledBy;
    private StateMachineRunner stateMachineRunner;

    private void Start()
    {
        characterControlledBy = GetComponent<CharacterControlledBy>();
        stateMachineRunner = GetComponent<StateMachineRunner>();
    }

    public void OnSwitchCharactersP1()
    {
        if(characterControlledBy.Player == CharacterControlledBy.Players.P1)
        {
            Switch();
        }
    }

    public void OnSwitchCharactersP2()
    {
        if (characterControlledBy.Player == CharacterControlledBy.Players.P2)
        {
            Switch();
        }
    }

    private void Switch()
    {
        stateMachineRunner.PerformTransition("Switch");
    }
}
