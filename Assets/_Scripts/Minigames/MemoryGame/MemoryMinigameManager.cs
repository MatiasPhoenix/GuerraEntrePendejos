using System.Collections.Generic;
using UnityEngine;

public class MemoryMinigameManager : MonoBehaviour
{
    [Header("List/Deck")]
    public Queue<Vector2> _cardPositions = new Queue<Vector2>();
    [SerializeField] private MemoryGameCard _cardPrefab;
    
    private List<MemoryGameCard> _deckMemory;
    private List<MemoryGameCard> _secondDeckMemory;

    void Start()
    {
        _deckMemory = new List<MemoryGameCard>(5);
        _secondDeckMemory = new List<MemoryGameCard>(5);
        CreateCards();
        DeckConfiguration();
        ConfigPositions();
    }

    void CreateCards()
    {
        for (int i = 0; i < 5; i++)
        {
            MemoryGameCard card1 = Instantiate(_cardPrefab);
            MemoryGameCard card2 = Instantiate(_cardPrefab);

            _deckMemory.Add(card1);
            _secondDeckMemory.Add(card2);
        }
    }

    void DeckConfiguration()
    {
        List<char> symbols = new List<char>();

        // Genera i simboli unici
        for (int i = 0; i < 5; i++)
        {
            char symbol = RandomSymbol();
            symbols.Add(symbol);
        }

        // Assegna i simboli alle carte
        for (int j = 0; j < _deckMemory.Count; j++)
        {
            _deckMemory[j].SetupSymbolCard(symbols[j]);
            _secondDeckMemory[j].SetupSymbolCard(symbols[j]); // Ora i simboli sono identici
        }

        // Mischia i due mazzi
        ShuffleDeck(_deckMemory);
        ShuffleDeck(_secondDeckMemory);
    }

    void ShuffleDeck(List<MemoryGameCard> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            MemoryGameCard temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    void ConfigPositions()
    {
        _cardPositions.Clear();

        Vector2 startPos = new Vector2(-3f, 1f);
        Vector2 currentPos = startPos;
        int cardsPerRow = 5;

        // Posiziona il primo mazzo
        for (int i = 0; i < _deckMemory.Count; i++)
        {
            _deckMemory[i].transform.position = currentPos;
            _cardPositions.Enqueue(currentPos);

            currentPos.x += 1.5f;
            if ((i + 1) % cardsPerRow == 0)
            {
                currentPos.x = startPos.x;
                currentPos.y -= 0f;
            }
        }

        // Aggiungi spazio tra i due mazzi
        currentPos.y -= 2f;

        // Posiziona il secondo mazzo
        for (int i = 0; i < _secondDeckMemory.Count; i++)
        {
            _secondDeckMemory[i].transform.position = currentPos;
            _cardPositions.Enqueue(currentPos);

            currentPos.x += 1.5f;
            if ((i + 1) % cardsPerRow == 0)
            {
                currentPos.x = startPos.x;
                currentPos.y -= 0f;
            }
        }
    }

    char RandomSymbol() => "abcdefghijklmnopqrstuvwxyz"[Random.Range(0, 26)];
}
