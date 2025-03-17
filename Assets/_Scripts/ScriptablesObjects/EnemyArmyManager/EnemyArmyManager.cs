using UnityEngine;

public class EnemyArmyManager : MonoBehaviour
{
    public string enemyID;
    public EnemyStateManager enemyStateManager;

    void Start()
    {
        if (enemyStateManager.IsEnemyDefeated(enemyID))
        {
            gameObject.SetActive(false); // Disattiva il nemico se è già stato sconfitto
        }
    }

    public void StartBattle()
    {
        enemyStateManager.SetLastBattlePosition(transform.position);
        // SceneManager.LoadScene("BattleScene");
    }

    public void OnDefeat()
    {
        enemyStateManager.MarkEnemyAsDefeated(enemyID);
        gameObject.SetActive(false);
    }
}
