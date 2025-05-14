using System.Collections.Generic;
using UnityEngine;

public class EternalMegaManager : MonoBehaviour
{
    public static EternalMegaManager Instance;

    //WarScore & objectives/mission manager
    private int _playerScore = 0;

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

    public int GetScore() => _playerScore;
    public void AddScore(int points) => _playerScore += points;



}
