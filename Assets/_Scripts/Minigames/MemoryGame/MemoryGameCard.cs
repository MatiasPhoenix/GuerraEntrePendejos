using TMPro;
using UnityEngine;

public class MemoryGameCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshPro _writeObject;
    [SerializeField] private GameObject _retroCard;

    public bool flipCard = false;

    public void SetupSymbolCard(char symbol) => _writeObject.text = symbol.ToString();
    public char WhatASymbol() => _writeObject.text[0];

    public void FlipCard()
    {
        flipCard = !flipCard;
        _retroCard.SetActive(!flipCard);
        _writeObject.gameObject.SetActive(flipCard);
    }

    private void OnMouseDown()
    {
        if (DeckManager.Instance.firstCard == true && DeckManager.Instance.secondCard == true) return;

        if (!flipCard && (DeckManager.Instance.firstCard == false || DeckManager.Instance.secondCard == false) && MemoryMinigameManager.Instance.currentPhase == MemoryPhaseManager.ChooseCard)
        {
            if (!DeckManager.Instance.firstCard)
            {
                DeckManager.Instance.cardA = WhatASymbol();
                DeckManager.Instance.firstCard = true;
            }
            else if (!DeckManager.Instance.secondCard)
            {
                DeckManager.Instance.cardB = WhatASymbol();
                DeckManager.Instance.secondCard = true;
            }

            FlipCard();
        }

        if (DeckManager.Instance.firstCard && DeckManager.Instance.secondCard)
            DeckManager.Instance.CheckMatchCalculation();
    }
}
