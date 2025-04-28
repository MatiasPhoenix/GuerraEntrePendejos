using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "EnemyStateManager", menuName = "Game/EnemyStateManager")]
public class EnemyStateManager : ScriptableObject
{
    [System.Serializable]
    public class EnemyState
    {
        public string enemyID;
        public bool isDefeated;
        public int scenario;
    }

    public List<EnemyState> enemyStatesFirstScene = new List<EnemyState>();
    public List<EnemyState> enemyStatesSecondScene = new List<EnemyState>();
    public Vector3 lastBattlePosition;  // Per il punto di respawn del giocatore
    ModifyCoorForBattleReturn modifyCoorForBattleReturn;
    public int currentScene;

    public void MarkEnemyAsDefeated(string enemyID)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        foreach (var enemy in enemyStatesFirstScene)
        {
            if (currentSceneIndex == 3) currentScene = enemy.scenario;

            if (enemy.enemyID == enemyID)
            {
                enemy.isDefeated = true;
                return;
            }
        }
        foreach (var enemy in enemyStatesSecondScene)
        {
            if (currentSceneIndex == 4) currentScene = enemy.scenario;

            if (enemy.enemyID == enemyID)
            {
                enemy.isDefeated = true;
                return;
            }
        }
    }

    public bool IsEnemyDefeated(string enemyID)
    {
        foreach (var enemy in enemyStatesFirstScene)
        {
            if (enemy.enemyID == enemyID)
                return enemy.isDefeated;
        }
        foreach (var enemy in enemyStatesSecondScene)
        {
            if (enemy.enemyID == enemyID)
                return enemy.isDefeated;
        }
        return false;
    }

    public void SetLastBattlePosition(Vector3 position)
    {
        lastBattlePosition = position;
        // SceneManager.LoadScene(currentScene);
    }

}
