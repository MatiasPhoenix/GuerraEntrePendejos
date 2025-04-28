using UnityEditor.SearchService;
using UnityEngine;

public class EnemyArmyManager : MonoBehaviour
{
    [Header("References Enemy Name & SO")]
    public string enemyID;
    public EnemyStateManager enemyStateManager;
    public GameObject respawnPlayerAfterBattle;

    void Start()
    {
        respawnPlayerAfterBattle.transform.position = enemyStateManager.lastBattlePosition;
        
        if (enemyStateManager.IsEnemyDefeated(enemyID))
            gameObject.SetActive(false);
        
    }

    public void StartBattle() => enemyStateManager.SetLastBattlePosition(transform.position);
    public void OnDefeat()
    {
        enemyStateManager.MarkEnemyAsDefeated(enemyID);
        // gameObject.SetActive(false);
    }
}
