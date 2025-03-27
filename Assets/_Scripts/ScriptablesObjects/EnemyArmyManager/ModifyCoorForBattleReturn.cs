using UnityEngine;

public class ModifyCoorForBattleReturn : MonoBehaviour
{
    public EnemyStateManager enemyStateManager;
    void Awake()
    {
        gameObject.transform.localPosition = enemyStateManager.lastBattlePosition;
    }
}
