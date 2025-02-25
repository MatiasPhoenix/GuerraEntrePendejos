using System.Collections;
using UnityEngine;

public class MemoryMinigameManager : MonoBehaviour
{
    public static MemoryMinigameManager Instance;
    public MemoryPhaseManager currentPhase = MemoryPhaseManager.ChooseCard;
    private bool _matchResult;

    private void Awake() => Instance = this;

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
    }

    public bool IsMatch(bool matchOrNot) => _matchResult = matchOrNot;

}

public enum MemoryPhaseManager
{
    ChooseCard,
    MatchOrNot,
    RemoveCard,
    FlipCards
}
