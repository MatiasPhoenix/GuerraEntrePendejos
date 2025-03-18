using System.Collections.Generic;
using UnityEngine;

public class HelperSceneManager : MonoBehaviour
{
    public static HelperSceneManager Instance;
    private void Awake() => Instance = this;

    [Header("Player team")]
    [SerializeField] private List<HeroUnit> _heroesTeam = new List<HeroUnit>();
    private int _battleNumberScenario = 0;
    private int _battleScenarioDecoration = 0;
    private List<EnemyUnit> _enemieTeam = new List<EnemyUnit>();

    public void BattleNumberCreate(int battleNumber, int scenarioDecoration)
    {
        _battleScenarioDecoration = scenarioDecoration;
        _battleNumberScenario = battleNumber;
    }
    public int BattleNumberScenarioGetter() => _battleNumberScenario;
    public int BattleScenarioDecorationGetter() => _battleScenarioDecoration;

    public List<EnemyUnit> EnemieTeamGetter() => _enemieTeam;
    public void EnemiesPartyCreate(List<EnemyUnit> EnemiesCurrentBattle) => _enemieTeam = EnemiesCurrentBattle;
    public void EmptyEnemCurrentBattle() => _enemieTeam.Clear();
    
    public List<HeroUnit> HeroesGetter() => _heroesTeam;
    


}
