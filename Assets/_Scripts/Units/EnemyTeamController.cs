using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamController : MonoBehaviour
{
    [Header("Active scegnario for battle")]
    [SerializeField] private int _battleNumberScenario = 0;
    [SerializeField] private int _battleScenarioDecoration = 0;
    [SerializeField] private int _scenarioPostBattle = 0;

    [Header("Enemy team for battle")]
    [SerializeField] private List<EnemyUnit> _enemieTeam = new List<EnemyUnit>();


    public void SetScenarioNumber()
    {
        HelperSceneManager.Instance.BattleNumberCreate(_battleNumberScenario, _battleScenarioDecoration);
        HelperSceneManager.Instance.ConfigNumberScenarioForBattleReturn(_scenarioPostBattle);
    }

    public void SetEnemyTeam() => HelperSceneManager.Instance.EnemiesPartyCreate(_enemieTeam);

}
