using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;

public class ReturnBattleManager : MonoBehaviour
{
    public static ReturnBattleManager Instance;
    private Vector2 _positionAfterBattle;
    private void Awake() => Instance = this;

    public void SetCoordinatesForVector2()
    {
        GameObject playerPosition = GameObject.FindWithTag("Player");
        Vector2 newPosition = new Vector2(playerPosition.transform.position.x + -2f, playerPosition.transform.position.y);
        SetPositionForBattleReturn(newPosition);
    }
    public void SetPositionForBattleReturn(Vector2 position) => _positionAfterBattle = position;
    public void ConfigObjectPosition() => gameObject.transform.position = _positionAfterBattle;
    public void ReturnToAdventurePhase() => GameManager.Instance.ChangeState(GameState.AdventurePhase);


}
