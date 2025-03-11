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
                paths.Add(hero, path);
        }

        if (paths.Count == 0)
        {
            Debug.Log($"Nessun bersaglio raggiungibile per {enemy.FactionAndName()}. Provo ad avvicinarmi.");
            yield return StartCoroutine(MoveCloserToHero(enemy)); // Aspetta la fine del movimento
            yield break;
        }

        var shortestPath = paths.OrderBy(p => p.Value.Count).First();
        _heroTarget = shortestPath.Key;

        Debug.Log($"---Bersaglio scelto: {_heroTarget.FactionAndName()} con percorso di {shortestPath.Value.Count} passi.");
        yield return StartCoroutine(GoToTarget(enemy)); // Aspetta la fine del movimento
    }


    private IEnumerator MoveCloserToHero(EnemyUnit enemy)
    {
        Dictionary<HeroUnit, List<NodeBase>> paths = new Dictionary<HeroUnit, List<NodeBase>>();

        foreach (HeroUnit hero in SpawnManager.Instance.GetHeroList())
        {
            var enemyLocation = GridManager.Instance.GetTileAtPosition(enemy.transform.position);
            var heroLocation = GridManager.Instance.GetTileAtPosition(hero.transform.position);
            var path = Pathfinding.FindAlternativePath(enemyLocation, heroLocation);

            if (path != null && path.Count > 0)
                paths.Add(hero, path);
        }

        if (paths.Count == 0)
        {
            Debug.Log($"{enemy.FactionAndName()} non trova percorsi alternativi. Turno terminato.");
            _enemyTurnFinished = true;
            yield break;
        }

        var shortestPath = paths.OrderBy(p => p.Value.Count).First();
        _heroTarget = shortestPath.Key;

        Debug.Log($"---PERCORSO ALTERNATIVO, con {shortestPath.Value.Count} passi.");

        AnimationManager.Instance.PlayWalkAnimation(enemy, true);
        _enemyTurnFinished = false;

        List<NodeBase> pathToFollow = shortestPath.Value;
        pathToFollow.Reverse();

        foreach (var tile in pathToFollow)
        {
            if (Vector2.Distance(enemy.transform.position, _heroTarget.transform.position) <= enemy.MaxAttack())
            {
                StartCoroutine(AttackAction(enemy));
                yield break;
            }

            if (enemy.CurrentMovement() <= 0)
            {
                break; // Se il nemico ha finito il movimento, esce dal ciclo
            }

            yield return new WaitForSeconds(0.15f);
            if (tile.Walkable == true && tile.OccupateByEnemy == false && tile.OccupateByUnit == false)
                enemy.transform.position = tile.Coords.Pos;
            else
            {
                if(enemy.transform.position.y < tile.transform.position.y && tile.Walkable)
                    enemy.transform.position += Vector3.up;
                else if(enemy.transform.position.y > tile.transform.position.y && tile.Walkable)
                    enemy.transform.position += Vector3.down;

                tile.RevertTile();
                enemy.MovementModifier(false);
                break;
            }
            tile.RevertTile();
            enemy.MovementModifier(false);
        }

        EnemyFinishedMovement(enemy);
        _enemyTurnFinished = true;
    }



    private IEnumerator GoToTarget(EnemyUnit currentEnemy, NodeBase targetNode = null)
    {
        AnimationManager.Instance.PlayWalkAnimation(currentEnemy, true);
        _enemyTurnFinished = false;

        if (targetNode == null)
            targetNode = GridManager.Instance.GetTileAtPosition(_heroTarget.transform.position);

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
            if (tile.Walkable)
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