using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStateManager", menuName = "Game/EnemyStateManager")]
public class EnemyStateManager : ScriptableObject
{
    [System.Serializable]
    public class EnemyState
    {
        public string enemyID;
        public bool isDefeated;
    }

    public List<EnemyState> enemyStates = new List<EnemyState>();
    public Vector3 lastBattlePosition;  // Per il punto di respawn del giocatore

    public void MarkEnemyAsDefeated(string enemyID)
    {
        foreach (var enemy in enemyStates)
        {
            if (enemy.enemyID == enemyID)
            {
                enemy.isDefeated = true;
                return;
            }
        }
    }

    public bool IsEnemyDefeated(string enemyID)
    {
        foreach (var enemy in enemyStates)
        {
            if (enemy.enemyID == enemyID)
                return enemy.isDefeated;
        }
        return false;
    }

    public void SetLastBattlePosition(Vector3 position)
    {
        lastBattlePosition = position;
    }
}
