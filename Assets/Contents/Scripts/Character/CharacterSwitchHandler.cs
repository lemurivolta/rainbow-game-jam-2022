using SimpleStateMachine;
using UnityEngine;

public class CharacterSwitchHandler : MonoBehaviour
{
    private CharacterInfo characterInfo;
    private StateMachineRunner stateMachineRunner;
    private bool inExchangeDistance;

    private void Start()
    {
        characterInfo = GetComponent<CharacterInfo>();
        stateMachineRunner = GetComponent<StateMachineRunner>();
    }

    public void OnExchangeEnabled(bool enabled)
    {
        inExchangeDistance = enabled;
    }

    public void OnSwitchCharactersP1()
    {
        if(characterInfo.Player == CharacterInfo.Players.P1 &&
            !inExchangeDistance)
        {
            Switch();
        }
    }

    public void OnSwitchCharactersP2()
    {
        if (characterInfo.Player == CharacterInfo.Players.P2 &&
            !inExchangeDistance)
        {
            Switch();
        }
    }

    private void Switch()
    {
        stateMachineRunner.PerformTransition("Switch");
    }
}
