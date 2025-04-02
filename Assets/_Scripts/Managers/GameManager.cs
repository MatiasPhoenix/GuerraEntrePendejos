using JetBrains.Annotations;
using Tarodev_Pathfinding._Scripts.Grid;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int MinigameIndexLocal;

    private void Awake()
    {
        Instance = this;
        // ChangeState(GameState.AdventurePhase);
    }

    private bool _heroesWin = true;

    [Header("Game State Manager")]
    public GameState GameState;
    public TextMeshProUGUI PlayerScore;

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.AdventurePhase:
                Debug.LogWarning("---Adventure Phase");
                PauseGameManager();
                break;
            case GameState.AfterBattle:
                Debug.LogWarning("---After Battle");
                ReturnBattleManager.Instance.SpawnPointConfiguration();
                break;
            case GameState.MenuOptions:
                Debug.LogWarning("---Menu Options");
                break;
            case GameState.GameStart:
                Debug.LogWarning("---Inizia GeneratedGrid!");
                // GridManager.Instance.GeneratedGrid();
                // GridManager.Instance.CreateMapGame();
                break;
            case GameState.PlayerSpawn:
                Debug.LogWarning("---Inizia SpawnHeroes!");
                GridManager.Instance.SpawnUnitsForGame();
                // SpawnManager.Instance.PopulateUnitLists();
                ChangeState(GameState.EnemySpawn);
                Debug.LogWarning($"{SpawnManager.Instance.GetHeroList().Count} Spawned!");
                break;
            case GameState.EnemySpawn:
                Debug.LogWarning("---Inizia SpawnEnemies!");
                GridManager.Instance.SpawnUnitsForGame();
                // SpawnManager.Instance.PopulateUnitLists();
                ChangeState(GameState.OrganizationPhase);
                break;
            case GameState.OrganizationPhase:
                Debug.LogWarning("---Inizia Organizzazione!");
                break;
            case GameState.PlayerTurn:
                Debug.LogWarning("--------------------PLAYER TURN!--------------------");
                BattleManager.Instance.BattleWinnerCalculator();
                SpawnManager.Instance.FindObjectsSlotDragDrop();
                SpawnManager.Instance.PopulateUnitLists();
                GridManager.Instance.UpdateTiles();
                SpawnManager.Instance.ResetMovementOfUnits();
                CanvasManager.Instance.ShowActiveTurnPanel();
                break;
            case GameState.EnemyTurn:
                Debug.LogWarning("--------------------ENEMY TURN!--------------------");
                // SpawnManager.Instance.PopulateUnitLists();
                GridManager.Instance.UpdateTiles();
                SpawnManager.Instance.ResetMovementOfUnits();
                CanvasManager.Instance.ShowActiveTurnPanel();
                EnemyManager.Instance.BeginEnemyTurns();
                break;
            case GameState.FinishBattle:
                Debug.LogWarning("---Battle Finito!");
                GridManager.Instance.UpdateTiles();
                SpawnManager.Instance.ResetMovementOfUnits();
                CanvasManager.Instance.ShowActiveTurnPanel();
                SpawnManager.Instance.DestroyAllHeroesAndEnemiesInBattleScene();
                break;
            case GameState.MinigamePhase:
                Debug.LogWarning("---Minigame Phase!");
                break;
            case GameState.PauseGame:
                Debug.LogWarning("---Pause Game!");
                PauseGameManager();
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
        int currentScore = EternalMegaManager.Instance.GetScore();
        int newScore = currentScore + scoreNumber;

        EternalMegaManager.Instance.AddScore(scoreNumber); // Aggiunge solo il punteggio guadagnato

        if (PlayerScore == null)
            PlayerScore = GameObject.FindGameObjectWithTag("WarScore").GetComponent<TextMeshProUGUI>();

        if (PlayerScore != null)
            PlayerScore.text = $"War Score: {newScore}";
        else
            Debug.LogError("⚠️ PlayerScore è null! Assicurati che sia assegnato correttamente.");

    }
    public void PauseGameManager()
    {
        var playerReference = GameObject.FindGameObjectWithTag("Player");
        var playerScript = playerReference.GetComponent<PlayerController>();

        if(GameState == GameState.PauseGame) playerScript.enabled = false;
        else playerScript.enabled = true;
    }

}


public enum GameState
{
    AdventurePhase,
    AfterBattle,
    GameStart,
    MenuOptions,
    PlayerSpawn,
    EnemySpawn,
    OrganizationPhase,
    PlayerTurn,
    EnemyTurn,
    FinishBattle,
    PauseGame,
    MinigamePhase,

}
