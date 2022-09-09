using UnityEngine;
using UnityEngine.Events;

public class EnemyFaint : MonoBehaviour
{
    public UnityEvent Fainted;

    public void OnPlayer1Action() {
        OnPlayerAction(CharacterInfo.Players.P1);
    }

    public void OnPlayer2Action() {
        OnPlayerAction(CharacterInfo.Players.P2);
    }

    private void OnPlayerAction(CharacterInfo.Players p)
    {
        foreach(var character in CharacterInfo.AllCharacterControlledBy) {
            if(character.Player == p && !character.IsFollower) {
                if(character.Character == CHARACTER.MARCELLA) {
                    Faint();
                } else {
                    break;
                }
            }
        }
    }

    private void Faint()
    {
        Fainted.Invoke();
    }
}
