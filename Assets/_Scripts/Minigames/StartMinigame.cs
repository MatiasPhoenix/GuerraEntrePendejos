using System;
using UnityEngine;

public class StartMinigame : MonoBehaviour
{
    public static event Action OnStartNewMinigame;

    [Header("Start Minigame")]
    [SerializeField] private int _minigameNumber;

    public void StartNewMinigame()
    {
        GameManager.Instance.MinigameIndexLocal = _minigameNumber;
        OnStartNewMinigame?.Invoke();
    }

}
