using System.Collections.Generic;
using System.Linq;
using _Scripts.Tiles;
using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("HERO Units")]
    [SerializeField] private List<HeroUnit> _heroePrefabs = new List<HeroUnit>(); //LIST of all heroes
    [SerializeField] private List<GameObject> _squareChooseZone = new List<GameObject>();
    [SerializeField] private GameObject _heroesChooseZone;
    [SerializeField] private GameObject _chooseHeroMessage;
    [SerializeField] private GameObject _ReadyForBattleButton;

    [Header("ENEMY Units")]
    [SerializeField] private List<EnemyUnit> _enemyPrefabs = new List<EnemyUnit>();


    //LIST of characters in game
    private List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();
    private List<HeroUnit> _heroUnits = new List<HeroUnit>();


    void Awake() => Instance = this;

    void Start() => UpdateEnemyAndHeroLists();


    public void PopulateUnitLists()
    {
        _enemyUnits = FindObjectsByType<EnemyUnit>(FindObjectsSortMode.None).ToList();
        _heroUnits = FindObjectsByType<HeroUnit>(FindObjectsSortMode.None).ToList();
    }

    public List<HeroUnit> GetHeroList() => _heroUnits;
    public List<EnemyUnit> GetEnemyList() => _enemyUnits;
    public void ChooseZoneOff()
    {
        _heroesChooseZone.SetActive(false);
        _chooseHeroMessage.SetActive(false);
        _ReadyForBattleButton.SetActive(false);
        StartCoroutine(CanvasManager.Instance.GameMessageStartOrEnd("Start"));
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

    private void UpdateEnemyAndHeroLists()
    {
        _heroePrefabs = HelperSceneManager.Instance.HeroesGetter();
        _enemyPrefabs = HelperSceneManager.Instance.EnemieTeamGetter();
    }
    public void ChooseTileForSpawnUnits() //Select the tile to spawn the units
    {
        if (GameManager.Instance.GameState == GameState.PlayerSpawn)
        {
            // for (int i = 0; i < _heroePrefabs.Count; i++)
            // {
            //     NodeBase playerNodeBase = GridManager.Instance.TileForTeams();
            //     Instantiate(_heroePrefabs[i], playerNodeBase.Coords.Pos, _heroePrefabs[i].transform.rotation);
            //     GridManager.Instance.UpdateTiles();
            // }
            SetHeroesInChooseArea();
        }

        if (GameManager.Instance.GameState == GameState.EnemySpawn)
        {
            for (int i = 0; i < _enemyPrefabs.Count; i++)
            {
                NodeBase enemyNodeBase = GridManager.Instance.TileForTeams();
                Instantiate(_enemyPrefabs[i], enemyNodeBase.Coords.Pos, _enemyPrefabs[i].transform.rotation);
                GridManager.Instance.UpdateTiles();
            }
        }
    }

    public void ResetMovementOfUnits()
    {
        List<HeroUnit> allHeroes = FindObjectsByType<HeroUnit>(FindObjectsSortMode.None).ToList();
        List<EnemyUnit> allEnemies = FindObjectsByType<EnemyUnit>(FindObjectsSortMode.None).ToList();

        if (GameManager.Instance.GameState == GameState.PlayerTurn)
        {
            foreach (var unit in allHeroes)
                unit.RestartMovement();
        }
        else if (GameManager.Instance.GameState == GameState.EnemyTurn)
        {
            foreach (var unit in allEnemies)
                unit.RestartMovement();
        }
    }

    void SetHeroesInChooseArea()
    {
        if (_heroePrefabs == null || _heroePrefabs.Count == 0)
        {
            Debug.LogError("⚠️ Errore: _heroePrefabs è null o vuota!");
            return;
        }

        if (_squareChooseZone == null || _squareChooseZone.Count == 0)
        {
            Debug.LogError("⚠️ Errore: _squareChooseZone è null o vuota!");
            return;
        }

        for (int i = 0; i < _heroePrefabs.Count; i++)
        {
            if (i >= _squareChooseZone.Count)
            {
                Debug.LogError($"⚠️ Errore: tentativo di accesso fuori dai limiti in _squareChooseZone! Index: {i}");
                break;
            }

            if (_squareChooseZone[i] == null)
            {
                Debug.LogError($"⚠️ Errore: _squareChooseZone[{i}] è null!");
                continue;
            }
            Instantiate(_heroePrefabs[i], _squareChooseZone[i].transform.position, _heroePrefabs[i].transform.rotation);

        }
    }

    public void HeroScriptActivationForBattle()
    {
        List<HeroUnit> heroInGame = FindObjectsByType<HeroUnit>(FindObjectsSortMode.None).ToList();

        // Scorre tutti gli eroi presenti nella lista
        foreach (var hero in heroInGame)
        {
            if (hero != null) // Controlla che l'oggetto non sia nullo
            {
                // Attiva o disattiva script specifici
                var dragAndDropScript = hero.GetComponent<DragAndDropObject>();
                var collider2D = hero.GetComponent<CapsuleCollider2D>();

                if (dragAndDropScript != null)
                    dragAndDropScript.enabled = false;

                if (collider2D != null)
                    collider2D.enabled = false;

            }
        }
    }

    public void FindObjectsSlotDragDrop()
    {
        List<HeroUnit> heroInGame = FindObjectsByType<HeroUnit>(FindObjectsSortMode.None).ToList();
        List<NodeBase> tilesInGame = FindObjectsByType<NodeBase>(FindObjectsSortMode.None).ToList();

        HashSet<Vector3> tilePositions = new HashSet<Vector3>(tilesInGame.Select(tile => tile.transform.position));

        foreach (HeroUnit unit in heroInGame)
        {
            bool isOnTile = tilePositions.Any(tilePos => Vector3.Distance(tilePos, unit.transform.position) < 0.1f);

            if (!isOnTile) // Se l'eroe non è su nessuna tile, lo distruggiamo
            {
                Debug.LogWarning($"❌ {unit.name} è fuori dalle tile e verrà distrutto!");
                Destroy(unit.gameObject);
            }
        }
    }



}
