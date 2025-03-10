using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Tiles;
using Tarodev_Pathfinding._Scripts;
using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private void Awake() => Instance = this;

    private HeroUnit _heroTarget;
    private bool _enemyTurnFinished = false;
    private bool _isAttacking = false; // Flag per evitare attacchi multipli

    public void BeginEnemyTurns() => StartCoroutine(ProcessEnemyTurns());

    private IEnumerator ProcessEnemyTurns()
    {
        SpawnManager.Instance.PopulateUnitLists();

        foreach (var enemy in SpawnManager.Instance.GetEnemyList())
        {
            _enemyTurnFinished = false;
            _isAttacking = false;

            Debug.Log($"Turno del nemico: {enemy.FactionAndName()}");
            StartCoroutine(TargetHeroToCombat(enemy));

            yield return new WaitUntil(() => _enemyTurnFinished);
        }
        yield return new WaitForSeconds(1f);
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

    private IEnumerator TargetHeroToCombat(EnemyUnit enemy)
    {
        Dictionary<HeroUnit, List<NodeBase>> paths = new Dictionary<HeroUnit, List<NodeBase>>();

        foreach (HeroUnit hero in SpawnManager.Instance.GetHeroList())
        {
            var enemyLocation = GridManager.Instance.GetTileAtPosition(enemy.transform.position);
            var heroLocation = GridManager.Instance.GetTileAtPosition(hero.transform.position);
            var path = Pathfinding.FindPath(enemyLocation, heroLocation);

            if (path != null && path.Count > 0)
            {
                Debug.Log($">>>Sono entrato nel if di {enemy.FactionAndName()} per muovere il nemico");
                paths.Add(hero, path);
            }
            else
                Debug.Log($">>>A quanto pare non succede una sega");

        }

        if (paths.Count == 0)
        {
            Debug.Log($"Nessun bersaglio raggiungibile per {enemy.FactionAndName()}. Provo ad avvicinarmi.");
            StartCoroutine(MoveCloserToHero(enemy));
            _enemyTurnFinished = true;
            yield break;
        }

        var shortestPath = paths.OrderBy(p => p.Value.Count).First();
        _heroTarget = shortestPath.Key;

        Debug.Log($"---Bersaglio scelto: {_heroTarget.FactionAndName()} con percorso di {shortestPath.Value.Count} passi.");
        yield return StartCoroutine(GoToTarget(enemy));
        // yield break;
    }

    private IEnumerator MoveCloserToHero(EnemyUnit enemy)
    {
        Debug.Log($"Nemico {enemy.FactionAndName()} cerca di avvicinarmi.");
        var enemyLocation = GridManager.Instance.GetTileAtPosition(enemy.transform.position);
        List<NodeBase> enemyAllyTile = SpawnManager.Instance.GetEnemyList()
            .Select(enemyAlly => GridManager.Instance.GetTileAtPosition(enemyAlly.transform.position))
            .ToList();

        NodeBase bestTarget = null;
        int shortestDistance = int.MaxValue;

        foreach (var tile in enemyAllyTile)
        {
            var path = Pathfinding.FindPath(enemyLocation, tile);
            if (path != null && path.Count > 0 && path.Count < shortestDistance)
            {
                bestTarget = tile;
                shortestDistance = path.Count;
            }
        }

        if (bestTarget != null)
        {
            Debug.Log($"Nemico {enemy.FactionAndName()} si avvicina verso {bestTarget.Coords.Pos}.");
            yield return StartCoroutine(GoToTarget(enemy, bestTarget));
            yield break;
        }
    }


    private IEnumerator GoToTarget(EnemyUnit currentEnemy, NodeBase targetNode = null)
    {
        Debug.Log($"Entro in GoToTarget di ----> {currentEnemy.FactionAndName()}");
        AnimationManager.Instance.PlayWalkAnimation(currentEnemy, true);
        _enemyTurnFinished = false;

        if (targetNode == null)
        {
            targetNode = GridManager.Instance.GetTileAtPosition(_heroTarget.transform.position);
        }

        var enemyLocation = GridManager.Instance.GetTileAtPosition(currentEnemy.transform.position);
        var path = Pathfinding.FindPath(enemyLocation, targetNode);

        if (path == null || path.Count == 0)
        {
            Debug.Log($"Nemico {currentEnemy.FactionAndName()} non può raggiungere il bersaglio, movimento terminato.");
            GridManager.Instance.UpdateTiles();
            _enemyTurnFinished = true;
            yield break;
        }

        path.Reverse();
        targetNode.RevertTile();

        foreach (var tile in path)
        {
            if (Vector2.Distance(currentEnemy.transform.position, _heroTarget.transform.position) <= currentEnemy.MaxAttack())
            {
                StartCoroutine(AttackAction(currentEnemy));
                yield break;
            }

            if (currentEnemy.CurrentMovement() <= 0)
            {
                break; // Se il nemico ha finito il movimento, esce dal ciclo
            }

            yield return new WaitForSeconds(0.15f);
            currentEnemy.transform.position = tile.Coords.Pos;
            tile.RevertTile();
            currentEnemy.MovementModifier(false);
        }

        EnemyFinishedMovement(currentEnemy);
        _enemyTurnFinished = true;
    }


    private IEnumerator AttackAction(EnemyUnit currentEnemy)
    {
        if (_isAttacking) yield break; // Evita attacchi multipli
        _isAttacking = true;

        EnemyFinishedMovement(currentEnemy);
        yield return StartCoroutine(BattleManager.Instance.StartBattle(currentEnemy, _heroTarget));
        _enemyTurnFinished = true;
    }

    private void EnemyFinishedMovement(EnemyUnit currentEnemy)
    {
        AnimationManager.Instance.PlayWalkAnimation(currentEnemy, false);
        Debug.Log($"{currentEnemy.FactionAndName()} ha terminato il movimento o ha raggiunto il bersaglio.");
        GridManager.Instance.UpdateTiles();
    }
}