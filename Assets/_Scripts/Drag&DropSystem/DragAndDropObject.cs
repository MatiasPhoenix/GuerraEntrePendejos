using UnityEngine;

public class DragAndDropObject : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private bool isDragging = false;
    
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
        isDragging = false;

        // Verifica se è sopra uno slot
        Collider2D slotCollider = Physics2D.OverlapPoint(transform.position);
        if (slotCollider != null)
        {
            SlotForHeroes slot = slotCollider.GetComponent<SlotForHeroes>();
            if (slot != null)
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
}
