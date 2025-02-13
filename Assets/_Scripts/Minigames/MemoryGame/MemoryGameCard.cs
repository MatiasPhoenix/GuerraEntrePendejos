using TMPro;
using UnityEngine;

public class MemoryGameCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshPro _writeObject;
    [SerializeField] private GameObject _retroCard;

    public void SetupSymbolCard(char symbol) => _writeObject.text = symbol.ToString();
}
