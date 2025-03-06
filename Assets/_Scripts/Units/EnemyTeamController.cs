using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamController : MonoBehaviour
{
    [Header("Active segnario for battle")]
    [SerializeField] private int _battleNumberScenario = 0;
    [Header("Enemy team for battle")]
    [SerializeField] private List<EnemyUnit> _enemieTeam = new List<EnemyUnit>();

    public void SetScenarioNumber() => HelperSceneManager.Instance.BattleNumberCreate(_battleNumberScenario);

    public void SetEnemyTeam() => HelperSceneManager.Instance.EnemiesPartyCreate(_enemieTeam);

}
