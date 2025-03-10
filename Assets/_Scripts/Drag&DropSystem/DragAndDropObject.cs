using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Tiles;
using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;

public class DragAndDropObject : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private bool isDragging = false;

    public static event Action OnItemDeselect;
    public static event Action<DragAndDropObject> OnItemSelect;

    [HideInInspector] public Transform OriginalParent;
    [HideInInspector] public Vector3 OriginalPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
        OriginalParent = transform.parent;
        OriginalPosition = transform.position;
    }

    private void OnMouseDown()
    {
        FindHeroAndConfigTile();
        OnItemSelect?.Invoke(this);

        if (mainCamera == null) return;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = transform.position - mousePosition;
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (!isDragging || mainCamera == null) return;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition + offset;
    }

    private void OnMouseUp()
    {
        OnItemDeselect?.Invoke();
        isDragging = false;

        // Verifica se è sopra uno slot
        Collider2D slotCollider = Physics2D.OverlapPoint(transform.position);
        if (slotCollider != null)
        {
            NodeBase thisTile = GridManager.Instance.GetTileAtPosition(slotCollider.transform.position);

            SlotForHeroes slot = slotCollider.GetComponent<SlotForHeroes>();
            if (slot != null && thisTile.Walkable != false && thisTile.ThisHero == null)
            {
                Debug.Log($"Oggetto spostato nel slot {slot}");
                slot.ReceiveItem(this);
            }
            else
            {
                Debug.Log($"Non è possibile spostare l'oggetto qui");
                ReturnToOriginalPosition();
            }
        }
        else
        {
            Debug.Log($"Nessun slot trovato");
            ReturnToOriginalPosition();
        }
    }

    public void ReturnToOriginalPosition()
    {
        transform.position = OriginalPosition;
        transform.SetParent(OriginalParent);
    }

    public void FindHeroAndConfigTile()
    {
        List<HeroUnit> heroInGame = FindObjectsByType<HeroUnit>(FindObjectsSortMode.None).ToList();
        List<NodeBase> tilesInGame = FindObjectsByType<NodeBase>(FindObjectsSortMode.None).ToList();

        // Pulisce solo i tile che non contengono più un eroe
        foreach (NodeBase tile in tilesInGame)
        {
            if (tile.ThisHero != null)
            {
                bool heroStillHere = heroInGame.Exists(hero => hero.transform.position == tile.transform.position);
                if (!heroStillHere)
                    tile.ThisHero = null; // Reset solo se il vecchio eroe si è mosso
            }
        }

        // Assegna gli eroi ai tile attuali
        foreach (HeroUnit unit in heroInGame)
        {
            foreach (NodeBase tile in tilesInGame)
            {
                if (unit.transform.position == tile.transform.position)
                {
                    tile.ThisHero = unit;
                    break; // Esce dal loop dopo aver assegnato
                }
            }
        }
    }



}
