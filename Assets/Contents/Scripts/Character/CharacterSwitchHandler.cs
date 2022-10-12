using SimpleStateMachine;
using UnityEngine;

public class CharacterSwitchHandler : MonoBehaviour
{
    private CharacterInfo characterInfo;
    private StateMachineRunner stateMachineRunner;

    private void Start()
    {
        characterInfo = GetComponent<CharacterInfo>();
        stateMachineRunner = GetComponent<StateMachineRunner>();
    }

    public void OnSwitchCharactersP1()
    {
        if (characterInfo.Player == CharacterInfo.Players.P1)
        {
            Switch();
        }
    }

    public void OnSwitchCharactersP2()
    {
        if (characterInfo.Player == CharacterInfo.Players.P2)
        {
            Switch();
        }
    }

    private void Switch()
    {
        stateMachineRunner.PerformTransition("Switch");
    }
}
