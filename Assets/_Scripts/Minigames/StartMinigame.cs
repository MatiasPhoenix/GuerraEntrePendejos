using UnityEngine;

public class StartMinigame : MonoBehaviour
{
    [Header("Start Minigame")]
    [SerializeField] private int _minigameNumber;

    public void StartNewMinigame()
    {
        GameManager.Instance.MinigameIndexLocal = _minigameNumber;
    }

}
