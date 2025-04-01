using System.Collections;
using TMPro;
using UnityEngine;

public class MemoryMinigameManager : MonoBehaviour
{
    public static MemoryMinigameManager Instance;
    public MemoryPhaseManager currentPhase = MemoryPhaseManager.ChooseCard;
    private bool _matchResult;
    private int _internalCounterForGame = 0;

    [Header("Control for end game")]
    [SerializeField] private GameObject _buttonObject;
    [SerializeField] private GameObject _timeFinishedGameObject;
    [SerializeField] private TextMeshPro _timeFinished;
    [SerializeField] private TextMeshPro _timer;
    [SerializeField] private float _timerNumber;


    private void Awake() => Instance = this;
    private void Update() => TimerUpdate();

    public void ChangePhaseMemorygame(MemoryPhaseManager phase)
    {
        currentPhase = phase;
        switch (currentPhase)
        {
            case MemoryPhaseManager.ChooseCard:
                ChooseCardPhase();
                break;

            case MemoryPhaseManager.MatchOrNot:
                CheckMatchPhase();
                break;

            case MemoryPhaseManager.RemoveCard:
                RemoveMatchedCards();
                break;

            case MemoryPhaseManager.FlipCards:
                ResetUnmatchedCards();
                break;
        }
    }

    void ChooseCardPhase() => Debug.Log("---Scegli una carta!");

    void CheckMatchPhase()
    {
        // Debug.Log("Controllo se c'è un match...");

        if (_matchResult)
            ChangePhaseMemorygame(MemoryPhaseManager.RemoveCard);
        else
            ChangePhaseMemorygame(MemoryPhaseManager.FlipCards);
    }

    void RemoveMatchedCards()
    {
        // Debug.Log("Rimuovendo carte");
        StartCoroutine(DelayedRestore());
    }

    void ResetUnmatchedCards()
    {
        // Debug.Log("Rigirando carte");
        StartCoroutine(DelayedRestore());
    }

    private IEnumerator DelayedRestore()
    {
        yield return new WaitForSeconds(1f);

        if (currentPhase == MemoryPhaseManager.FlipCards)
            DeckManager.Instance.RestoreCardPosition();
        else if (currentPhase == MemoryPhaseManager.RemoveCard)
            DeckManager.Instance.RemoveCardAfterMatch();

        _internalCounterForGame++;
        if (_internalCounterForGame == 5) GameOver();
        
    }

    public bool IsMatch(bool matchOrNot) => _matchResult = matchOrNot;
    public void GameOver()
    {
        ChangePhaseMemorygame(MemoryPhaseManager.GameOver);
        _buttonObject.SetActive(true);
        _timeFinishedGameObject.SetActive(true);
        _timeFinished.text = $"Time finished!";
    }
    void TimerUpdate()
    {
        if (_timerNumber <= 0)
        {
            GameOver();
            return;
        }
        _timerNumber -= Time.deltaTime;
        _timer.text = $"Timer: {(int)_timerNumber}";
    }
}

public enum MemoryPhaseManager
{
    ChooseCard,
    MatchOrNot,
    RemoveCard,
    FlipCards,
    GameOver
}
