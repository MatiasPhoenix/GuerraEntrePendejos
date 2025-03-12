using UnityEngine;

public class EternalMegaManager : MonoBehaviour
{
    public static EternalMegaManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //WarScore
    private int _playerScore = 0;

    public void AddScore(int points)
    {
        _playerScore += points;
        Debug.Log($"Punteggio aggiornato: {_playerScore}");
    }

    public int GetScore() => _playerScore;


}
