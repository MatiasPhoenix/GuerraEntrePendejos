using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Header("List/Deck")]
    public Queue<Vector2> _cardPositions = new Queue<Vector2>();
    [SerializeField] private MemoryGameCard _cardPrefab;

    [Header("Match or Fail")]
    [SerializeField] private TextMeshPro _matchText;
    [SerializeField] private TextMeshPro _failText;

    private List<MemoryGameCard> _deckMemory;
    private List<MemoryGameCard> _secondDeckMemory;

    [Header("--Don't Touch!!--")]
    public bool firstCard = false;
    public char cardA;
    public bool secondCard = false;
    public char cardB;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _deckMemory = new List<MemoryGameCard>();
        _secondDeckMemory = new List<MemoryGameCard>();

        CreateCards();
        DeckConfiguration();
        ConfigPositions();
        MemoryMinigameManager.Instance.ChangePhaseMemorygame(MemoryPhaseManager.ChooseCard);
    }

    void RestoreAllStats()
    {
        firstCard = false;
        secondCard = false;
        _matchText.gameObject.SetActive(false);
        _failText.gameObject.SetActive(false);
    }
    public void CheckMatchCalculation()
    {
        MemoryMinigameManager.Instance.IsMatch(CheckMatch());
        MemoryMinigameManager.Instance.ChangePhaseMemorygame(MemoryPhaseManager.MatchOrNot);
        if (CheckMatch())
        {
            _matchText.gameObject.SetActive(true);
            GameManager.Instance.WarScoreCounter(10);
        }
        else
        {
            _failText.gameObject.SetActive(true);
        }
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

        for (int i = 0; i < 5; i++)
        {
            symbols.Add(RandomSymbol());
        }

        for (int j = 0; j < _deckMemory.Count; j++)
        {
            _deckMemory[j].SetupSymbolCard(symbols[j]);
            _secondDeckMemory[j].SetupSymbolCard(symbols[j]);
        }

        ShuffleDeck(_deckMemory);
        ShuffleDeck(_secondDeckMemory);
    }

    void ShuffleDeck(List<MemoryGameCard> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (deck[i], deck[randomIndex]) = (deck[randomIndex], deck[i]);
        }
    }

    void ConfigPositions()
    {
        _cardPositions.Clear();

        Vector2 startPos = new Vector2(-3f, 1f);
        Vector2 currentPos = startPos;
        int cardsPerRow = 5;

        foreach (var card in _deckMemory)
        {
            card.transform.position = currentPos;
            _cardPositions.Enqueue(currentPos);

            currentPos.x += 1.5f;
            if (_cardPositions.Count % cardsPerRow == 0)
            {
                currentPos.x = startPos.x;
                currentPos.y -= 0f;
            }
        }

        currentPos.y -= 2f;

        foreach (var card in _secondDeckMemory)
        {
            card.transform.position = currentPos;
            _cardPositions.Enqueue(currentPos);

            currentPos.x += 1.5f;
            if (_cardPositions.Count % cardsPerRow == 0)
            {
                currentPos.x = startPos.x;
                currentPos.y -= 0f;
            }
        }
    }

    char RandomSymbol() => "abcdefghijklmnopqrstuvwxyz"[Random.Range(0, 26)];

    public bool CheckMatch() => cardA == cardB;

    public void RestoreCardPosition()
    {
        RestoreAllStats();

        foreach (var card in _deckMemory)
            if (card.flipCard)
                card.FlipCard();

        foreach (var card in _secondDeckMemory)
            if (card.flipCard)
                card.FlipCard();

        MemoryMinigameManager.Instance.ChangePhaseMemorygame(MemoryPhaseManager.ChooseCard);
    }

    public void RemoveCardAfterMatch()
    {
        RestoreAllStats();

        _deckMemory.RemoveAll(card =>
        {
            if (card.flipCard && MemoryMinigameManager.Instance.currentPhase == MemoryPhaseManager.RemoveCard)
            {
                Destroy(card.gameObject);
                return true;
            }
            return false;
        });

        _secondDeckMemory.RemoveAll(card =>
        {
            if (card.flipCard && MemoryMinigameManager.Instance.currentPhase == MemoryPhaseManager.RemoveCard)
            {
                Destroy(card.gameObject);
                return true;
            }
            return false;
        });
        MemoryMinigameManager.Instance.ChangePhaseMemorygame(MemoryPhaseManager.ChooseCard);
    }

    public void EmptyDeckMemory()
    {
        foreach (var card in _deckMemory)
            Destroy(card.gameObject);

        foreach (var card in _secondDeckMemory)
            Destroy(card.gameObject);
    }
}
