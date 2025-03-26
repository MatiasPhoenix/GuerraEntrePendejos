using UnityEngine;

public class ActivationZoneDialogueAndMore : MonoBehaviour
{
    [Header("GameObject Symbol")]
    public GameObject objectWithSymbol;

    void OnTriggerEnter2D(Collider2D collision)
    {
        objectWithSymbol.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        objectWithSymbol.SetActive(false);
    }
}
