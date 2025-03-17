using UnityEngine;

public class EnemyArmyManager : MonoBehaviour
{
    [Header("References Enemy Name & SO")]
    public string enemyID;
    public EnemyStateManager enemyStateManager;
    public GameObject respawnPlayerAfterBattle;

    void Start()
    {
        if (enemyStateManager.IsEnemyDefeated(enemyID))
            gameObject.SetActive(false);
        
        respawnPlayerAfterBattle.transform.position = enemyStateManager.lastBattlePosition;
    }

    public void StartBattle() => enemyStateManager.SetLastBattlePosition(transform.position);
    public void OnDefeat()
    {
        enemyStateManager.MarkEnemyAsDefeated(enemyID);
        gameObject.SetActive(false);
    }
}
