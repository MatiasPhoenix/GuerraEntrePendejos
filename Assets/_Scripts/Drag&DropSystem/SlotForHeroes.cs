using _Scripts.Tiles;
using Tarodev_Pathfinding._Scripts.Grid;
using UnityEngine;

public class SlotForHeroes : MonoBehaviour
{
    private DragAndDropObject _currentItem;

    void OnEnable()
    {
        
        DragAndDropObject.OnItemSelect += HandleItemSelected;
        DragAndDropObject.OnItemDeselect += HandleItemDeselected;
    }

    void OnDisable()
    {
        DragAndDropObject.OnItemSelect -= HandleItemSelected;
        DragAndDropObject.OnItemDeselect -= HandleItemDeselected;
    }

    public void ReceiveItem(DragAndDropObject draggedObject)
    {
        if (draggedObject == null) return;
        draggedObject.transform.position = transform.position;
    }

    private void HandleItemSelected(DragAndDropObject selectedItem)
    {
        _currentItem = selectedItem;
        Transform tileArea = transform.Find("TileArea");
        NodeBase thisTile = GetNodeBaseInfo();
        if (tileArea != null && (thisTile.Walkable != false || thisTile.ThisHero != null))
            tileArea.gameObject.SetActive(true);
    }

    private void HandleItemDeselected()
    {
        _currentItem = null;
        Transform tileArea = transform.Find("TileArea");
        if (tileArea != null)
            tileArea.gameObject.SetActive(false);
    }

    NodeBase GetNodeBaseInfo() => GridManager.Instance.GetTileAtPosition(gameObject.transform.position);

    

}
