using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;

public class ReturnBattleManager : MonoBehaviour
{
    public static ReturnBattleManager Instance;
    public EnemyStateManager enemyStateManager;

    private void Awake() => Instance = this;

    public void SpawnPointConfiguration()
    {
        gameObject.transform.position = enemyStateManager.lastBattlePosition;
        var playerGO = GameObject.FindGameObjectWithTag("WarScore");
        playerGO.transform.position = gameObject.transform.position;
    }
    
}
