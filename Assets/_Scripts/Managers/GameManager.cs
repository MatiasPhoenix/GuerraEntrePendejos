using Tarodev_Pathfinding._Scripts.Grid;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int MinigameIndexLocal;

    private void Awake() => Instance = this;

    private bool _heroesWin = true;

    void Start() => ChangeState(GameState.AdventurePhase);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ChangeState(GameState.PlayerTurn);

        if (Input.GetKeyDown(KeyCode.E))
            ChangeState(GameState.EnemyTurn);
    }

    [Header("Game State Manager")]
    public GameState GameState;
    public TextMeshProUGUI PlayerScore;

    private int _playerScore = 0;

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.AdventurePhase:
                Debug.Log("---Adventure Phase");
                break;
            case GameState.MenuOptions:
                Debug.Log("---Menu Options");
                break;
            case GameState.GameStart:
                Debug.Log("---Inizia GeneratedGrid!");
                // GridManager.Instance.GeneratedGrid();
                // GridManager.Instance.CreateMapGame();
                break;
            case GameState.PlayerSpawn:
                Debug.Log("---Inizia SpawnHeroes!");
                GridManager.Instance.SpawnUnitsForGame();
                // SpawnManager.Instance.PopulateUnitLists();
                ChangeState(GameState.EnemySpawn);
                Debug.Log($"{SpawnManager.Instance.GetHeroList().Count} Spawned!");
                break;
            case GameState.EnemySpawn:
                Debug.Log("---Inizia SpawnEnemies!");
                GridManager.Instance.SpawnUnitsForGame();
                // SpawnManager.Instance.PopulateUnitLists();
                ChangeState(GameState.OrganizationPhase);
                break;
            case GameState.OrganizationPhase:
            Debug.Log("---Inizia Organizzazione!");
                break;
            case GameState.PlayerTurn:
                Debug.Log("--------------------PLAYER TURN!--------------------");
                SpawnManager.Instance.FindObjectsSlotDragDrop();
                SpawnManager.Instance.PopulateUnitLists();
                GridManager.Instance.UpdateTiles();
                SpawnManager.Instance.ResetMovementOfUnits();
                CanvasManager.Instance.ShowActiveTurnPanel();
                BattleManager.Instance.BattleWinnerCalculator();
                break;
            case GameState.EnemyTurn:
                Debug.Log("--------------------ENEMY TURN!--------------------");
                // SpawnManager.Instance.PopulateUnitLists();
                GridManager.Instance.UpdateTiles();
                SpawnManager.Instance.ResetMovementOfUnits();
                CanvasManager.Instance.ShowActiveTurnPanel();
                BattleManager.Instance.BattleWinnerCalculator();
                EnemyManager.Instance.BeginEnemyTurns();
                break;

            default:
                Debug.LogError("Unhandled GameState: " + newState);
                break;
        }
    }

    public bool BattleResult(bool result) => _heroesWin = result;
    public bool BattleVictory() => _heroesWin;
    public void EndTurn() => ChangeState(GameState.EnemyTurn);
    public void WarScoreCounter(int scoreNumber)
    {
        _playerScore += scoreNumber;
        PlayerScore.text = $"War Score: {_playerScore}";
        Debug.Log($"Score: {_playerScore}");
    }
}


public enum GameState
{
    AdventurePhase,
    GameStart,
    MenuOptions,
    PlayerSpawn,
    EnemySpawn,
    OrganizationPhase,
    PlayerTurn,
    EnemyTurn,
    PausaGame,

}
