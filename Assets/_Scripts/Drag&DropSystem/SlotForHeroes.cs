using UnityEngine;

public class SlotForHeroes : MonoBehaviour
{
    public void ReceiveItem(DragAndDropObject draggedObject)
    {
        if (draggedObject == null) return;

        // Se c'è già un oggetto nello slot, lo rimanda alla posizione originale
        // if (transform.childCount > 0)
        // {
        //     GameObject currentItem = transform.GetChild(0).gameObject;
        //     ReturnItemToOriginalPosition(currentItem);
        // }

        // Assegna il nuovo oggetto allo slot
        // draggedObject.transform.SetParent(transform);
        draggedObject.transform.position = transform.position;
    }

    private void ReturnItemToOriginalPosition(GameObject item)
    {
        DragAndDropObject draggable = item.GetComponent<DragAndDropObject>();
        if (draggable != null)
        {
            draggable.ReturnToOriginalPosition();
        }
    }
}
