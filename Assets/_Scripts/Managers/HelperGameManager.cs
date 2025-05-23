using System.Collections.Generic;
using _Scripts.Tiles;
using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;

public class HelperGameManager : MonoBehaviour
{
    [Header("Battle Scenarios")]
    [SerializeField] private List<GameObject> _battleScenarios = new List<GameObject>();
    [SerializeField] private List<GameObject> _scenariosBackground = new List<GameObject>();
    [SerializeField] private List<GameObject> _battlegroundDecoration = new List<GameObject>();

    private void Start()
    {
        GoToBattle();
        CreateListOfTiles();
    }
    public void BattleReturn() => GameManager.Instance.ChangeState(GameState.FinishBattle);
    public void GoToBattle() => GameManager.Instance.ChangeState(GameState.GameStart);

    void CreateListOfTiles()
    {
        _battleScenarios[HelperSceneManager.Instance.BattleNumberScenarioGetter()].SetActive(true);
        _scenariosBackground[HelperSceneManager.Instance.BattleScenarioDecorationGetter()].SetActive(true);
        _battlegroundDecoration[HelperSceneManager.Instance.BattleNumberScenarioGetter()].SetActive(true);
        if (GridManager.Instance == null)
        {
            Debug.LogError("GridManager.Instance è null!");
            return;
        }

        if (GridManager.Instance.Tiles == null)
            GridManager.Instance.Tiles = new Dictionary<Vector2, NodeBase>(); // Inizializza solo una volta

        if (_battleScenarios == null || _battleScenarios.Count == 0 || _battleScenarios[HelperSceneManager.Instance.BattleNumberScenarioGetter()] == null)
        {
            Debug.LogError("Nessun Battle Scenario valido!");
            return;
        }

        foreach (var item in _battleScenarios[HelperSceneManager.Instance.BattleNumberScenarioGetter()].GetComponentsInChildren<NodeBase>())
        {
            Vector2 tileVector = item.transform.position;
            if (!GridManager.Instance.Tiles.ContainsKey(tileVector)) // Evita duplicati
                GridManager.Instance.Tiles.Add(tileVector, item);
        }

        foreach (var tile in GridManager.Instance.Tiles.Values)
        {
            bool isWalkable = !tile.MountainOrObstacle; // 👈 Imposta la walkability basata sul tipo di terreno
            tile.Init(isWalkable, new SquareCoords { Pos = tile.transform.position });
        }

        foreach (var tile in GridManager.Instance.Tiles.Values) tile.CacheNeighbors();
        GridManager.Instance.ActiveOnTileHoverForGame();

        GameManager.Instance.ChangeState(GameState.PlayerSpawn);

    }

    public void ClearTileList() => _battleScenarios.Clear();

}
