using UnityEngine;

public class HelperGameManager : MonoBehaviour
{
    private void Start()
    {
        GoToBattle();   
    }
    public void BattleReturn() => GameManager.Instance.ChangeState(GameState.AdventureBegin);
    public void GoToBattle() => GameManager.Instance.ChangeState(GameState.GameStart);
    
}
