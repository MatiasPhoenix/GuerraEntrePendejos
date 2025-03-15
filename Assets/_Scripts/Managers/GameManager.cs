using Tarodev_Pathfinding._Scripts.Grid;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int MinigameIndexLocal;

    private void Awake() => Instance = this;

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
                Debug.Log("---Adventure Phase");
                if (PlayerScore == null)
                    PlayerScore = GameObject.FindGameObjectWithTag("WarScore").GetComponent<TextMeshProUGUI>();
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
                BattleManager.Instance.BattleWinnerCalculator();
                SpawnManager.Instance.FindObjectsSlotDragDrop();
                SpawnManager.Instance.PopulateUnitLists();
                GridManager.Instance.UpdateTiles();
                SpawnManager.Instance.ResetMovementOfUnits();
                CanvasManager.Instance.ShowActiveTurnPanel();
                break;
            case GameState.EnemyTurn:
                Debug.Log("--------------------ENEMY TURN!--------------------");
                // SpawnManager.Instance.PopulateUnitLists();
                GridManager.Instance.UpdateTiles();
                SpawnManager.Instance.ResetMovementOfUnits();
                CanvasManager.Instance.ShowActiveTurnPanel();
                EnemyManager.Instance.BeginEnemyTurns();
                break;
            case GameState.FinishBattle:
                Debug.Log("---Battle Finito!");
                GridManager.Instance.UpdateTiles();
                SpawnManager.Instance.ResetMovementOfUnits();
                CanvasManager.Instance.ShowActiveTurnPanel();
                SpawnManager.Instance.DestroyAllHeroesAndEnemiesInBattleScene();
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

        if (PlayerScore != null)
        {
            PlayerScore.text = $"War Score: {newScore}";
        }
        else
        {
            Debug.LogError("⚠️ PlayerScore è null! Assicurati che sia assegnato correttamente.");
        }

        Debug.Log($"Score aggiornato: {newScore}");
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
    FinishBattle,
    PausaGame,

}
