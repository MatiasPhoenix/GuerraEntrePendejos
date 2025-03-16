using UnityEngine;

public class EternalMegaManager : MonoBehaviour
{
    public static EternalMegaManager Instance;

    //WarScore
    private int _playerScore = 0;
    private int _enemyNumberInScenario = 0;

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

    public void AddScore(int points)
    {
        _playerScore += points;
        Debug.Log($"Punteggio aggiornato: {_playerScore}");
    }

    public int GetScore() => _playerScore;
    public void SetNumberForEnemies(int number) => _enemyNumberInScenario = number;



}
