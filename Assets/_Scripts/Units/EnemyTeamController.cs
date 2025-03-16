using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamController : MonoBehaviour
{
    [Header("Active scegnario for battle")]
    [SerializeField] private int _battleNumberScenario = 0;

    [Header("Enemy team for battle")]
    [SerializeField] private List<EnemyUnit> _enemieTeam = new List<EnemyUnit>();

    [Header("Enemy number in scenario")]
    [SerializeField] private int _enemyNumberInScenario = 0;


    public void SetScenarioNumber() => HelperSceneManager.Instance.BattleNumberCreate(_battleNumberScenario);

    public void SetEnemyTeam() => HelperSceneManager.Instance.EnemiesPartyCreate(_enemieTeam);
    public void SetEnemyNumberInScenario() => EternalMegaManager.Instance.SetNumberForEnemies(_enemyNumberInScenario);

}
