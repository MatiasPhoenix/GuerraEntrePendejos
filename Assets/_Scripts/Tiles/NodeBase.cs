using System;
using System.Collections.Generic;
using Tarodev_Pathfinding._Scripts;
using Tarodev_Pathfinding._Scripts.Grid;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Tiles
{
    public abstract class NodeBase : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Color _obstacleColor;

        [SerializeField] private Gradient _walkableColor;
        [SerializeField] protected SpriteRenderer _renderer;


        public bool OccupateByUnit = false;
        public bool OccupateByEnemy = false;
        public GameObject TileForFloodFill;
        public HeroUnit ThisHero;
        public EnemyUnit ThisEnemy;
        public ICoords Coords;
        public float GetDistance(NodeBase other) => Coords.GetDistance(other.Coords); // Helper to reduce noise in pathfinding
        public bool Walkable;
        public bool MountainOrObstacle;
        private bool _selected;
        private Color _defaultColor;
        private Color _defaultTileForFloodFillColor;


        public virtual void Init(bool walkable, ICoords coords)
        {
            // Walkable = walkable;
            if (!Walkable)
                MountainOrObstacle = true;

            _renderer.color = walkable ? _walkableColor.Evaluate(Random.Range(0f, 1f)) : _obstacleColor;
            _defaultColor = _renderer.color;
            _defaultTileForFloodFillColor = TileForFloodFill.GetComponent<SpriteRenderer>().color;

            OnHoverTile += OnOnHoverTile;

            if (coords == null)
            {
                Coords = new SquareCoords { Pos = transform.position };  // 👈 Se `coords` è NULL, lo inizializziamo
                Debug.LogWarning($"⚠️ Coords era NULL, assegnato da transform.position: {Coords.Pos}");
            }
            else
            {
                Coords = coords;
                transform.position = Coords.Pos;  // 👈 Garantiamo che `Coords.Pos` sia uguale a `transform.position`
            }

            if (TileForFloodFill == null)
                TileForFloodFill = transform.Find("TileArea").gameObject;
            // Debug.Log($"✅ Inizializzazione completata per {gameObject.name}: Pos = {Coords.Pos}");
        }


        public static event Action<NodeBase> OnHoverTile;
        private void OnEnable() => OnHoverTile += OnOnHoverTile;
        private void OnDisable() => OnHoverTile -= OnOnHoverTile;
        private void OnOnHoverTile(NodeBase selected) => _selected = selected == this;

        protected virtual void OnMouseEnter()
        {
            if (MouseManager.Instance.HeroUnit != null && TileForFloodFill.activeSelf)
                OnHoverTile?.Invoke(this);

            if (MouseManager.Instance.attackPhase == true && TileForFloodFill.activeSelf && OccupateByEnemy)
            {
                ThisEnemy.ActiveUnitSelected();

                Debug.Log($"Qui c'è un ENEMY, {ThisEnemy.FactionAndName()} -> {transform.position}, walkable -> {Walkable} e mountain -> {MountainOrObstacle}");
            }

            if (ThisEnemy != null && CanvasManager.Instance.EnemyPanelIsActive() == false)
            {
                CanvasManager.Instance.SetActiveEnemyPanel();
                CanvasManager.Instance.UpgradePanelEnemyInfo(ThisEnemy);
            }
        }
        protected virtual void OnMouseExit()
        {
            if (ThisEnemy == null && CanvasManager.Instance.EnemyPanelIsActive() == true)
            {
                CancelAllEnemiesSelectedZone();
                CanvasManager.Instance.SetActiveEnemyPanel();
            }
        }
        void CancelAllEnemiesSelectedZone()
        {
            foreach (var enemy in SpawnManager.Instance.GetEnemyList())
                enemy.DesactiveUnitSelected();
        }

        protected void OnMouseDown()
        {
            if (MouseManager.Instance.attackPhase)
            {
                HandleAttackPhase();
                return;
            }

            MouseManager.Instance.attackPhase = false;

            if (GameManager.Instance.GameState == GameState.PlayerTurn)
            {
                if (MouseManager.Instance.HeroUnit != null)
                {
                    if (OccupateByUnit)
                    {
                        HandleUnitSelection();
                        return;
                    }
                    if(TileForFloodFill.activeSelf)
                    HandleUnitMovement();
                }
                else
                {
                    HandleUnitSelection();
                }
            }

            HandleTileDebug();
        }

        private void HandleAttackPhase()
        {
            if (OccupateByEnemy && TileForFloodFill.activeSelf)
            {
                StartCoroutine(BattleManager.Instance.StartBattle(MouseManager.Instance.HeroUnit, ThisEnemy));
            }
            else
            {
                UnitDeselectedInNodeBase();
            }
        }

        private void HandleUnitMovement()
        {
            if (TileForFloodFill.activeSelf && MouseManager.Instance.unitCanMoving == true)
            {
                MouseManager.Instance.MethodToMoveUnit();
            }
            else if(MouseManager.Instance.unitCanMoving == true)
            {
                Debug.Log($"Non puoi muovere più di {MouseManager.Instance.HeroUnit.MaxMovement()}");
                UnitDeselectedInNodeBase();
            }
        }

        private void HandleUnitSelection()
        {
            GridManager.Instance.UpdateTiles();

            if (OccupateByUnit && MouseManager.Instance.unitCanMoving == true)
            {
                // Se c'è già un'unità selezionata, la deselezioniamo prima di selezionarne una nuova
                if (MouseManager.Instance.HeroUnit != null)
                {
                    UnitDeselectedInNodeBase();
                }

                // Selezioniamo la nuova unità
                MouseManager.Instance.HeroUnit = ThisHero;
                CanvasManager.Instance.SetActiveHeroPanel();
                SelectUnitPlusNode(this, ThisHero);
                return;
            }

            // Debug nemico
            if (OccupateByEnemy)
            {
                Debug.Log($"Nemico trovato: {ThisEnemy.FactionAndName()} -> {transform.position}, walkable -> {Walkable}, ostacolo -> {MountainOrObstacle}");
            }
        }


        private void HandleTileDebug()
        {
            if (!OccupateByUnit && !OccupateByEnemy)
            {
                Debug.Log($"TILE VUOTO -> {transform.position}, walkable -> {Walkable}, ostacolo -> {MountainOrObstacle}");
            }
        }



        public void VisualizeFloodFill()
        {
            TileForFloodFill.SetActive(true);
            if (MouseManager.Instance.attackPhase == true)
            {
                SpriteRenderer spriteRenderer = TileForFloodFill.GetComponent<SpriteRenderer>();
                Color newColor = Color.red;
                newColor.a = 0.2f;
                spriteRenderer.color = newColor;
            }
        }
        public void HideFloodFill()
        {
            if (TileForFloodFill == null)
                TileForFloodFill = transform.Find("TileArea").gameObject;
            TileForFloodFill.GetComponent<SpriteRenderer>().color = _defaultTileForFloodFillColor;
            TileForFloodFill.SetActive(false);
        }

        public void UnitDeselectedInNodeBase()
        {
            MouseManager.Instance.attackPhase = false;
            GridManager.Instance.UnitDeselected();
            MouseManager.Instance.CancelSelectedUnit();
            CanvasManager.Instance.SetActiveHeroPanel();
            AreaMovementAndAttack.ResetFloodFill();
        }

        public void SelectUnitPlusNode(NodeBase baseNode, HeroUnit unit)
        {
            MouseManager.Instance.MouseInteraction(baseNode, unit);
        }


        #region Pathfinding

        [Header("Pathfinding")]
        [SerializeField]
        private TextMeshPro _fCostText;

        [SerializeField] private TextMeshPro _gCostText, _hCostText;
        public List<NodeBase> Neighbors { get; protected set; }
        public NodeBase Connection { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        public abstract void CacheNeighbors();

        public void SetConnection(NodeBase nodeBase)
        {
            Connection = nodeBase;
        }

        public void SetG(float g)
        {
            G = g;
            SetText();
        }

        public void SetH(float h)
        {
            H = h;
            SetText();
        }

        private void SetText()
        {
            if (_selected) return;
            _gCostText.text = G.ToString();
            _hCostText.text = H.ToString();
            _fCostText.text = F.ToString();
        }

        public void SetColor(Color color) => _renderer.color = color;

        public void RevertTile()
        {
            _renderer.color = _defaultColor;
            _gCostText.text = "";
            _hCostText.text = "";
            _fCostText.text = "";
        }

        #endregion

    }


}


public interface ICoords
{
    public float GetDistance(ICoords other);
    public Vector2 Pos { get; set; }
}