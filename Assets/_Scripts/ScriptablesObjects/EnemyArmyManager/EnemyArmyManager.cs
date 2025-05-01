using UnityEngine;

public class EnemyArmyManager : MonoBehaviour
{
    [Header("References Enemy Name & SO")]
    public string EnemyID;
    public EnemyStateManager EnemyStateManager;
    public GameObject RespawnPlayerAfterBattle;
    public int NumberScenePostBattle;

    void Start()
    {        
        RespawnPlayerAfterBattle.transform.position = EnemyStateManager.lastBattlePosition;

        if (EnemyStateManager.IsEnemyDefeated(EnemyID))
            gameObject.SetActive(false);

    }

    public void StartBattle() => EnemyStateManager.SetLastBattlePosition(transform.position);
    public void OnDefeat()
    {
        EnemyStateManager.MarkEnemyAsDefeated(EnemyID);
        // GameManager.Instance.ChangeNumberScene(NumberScenePostBattle);
    }
   
}
