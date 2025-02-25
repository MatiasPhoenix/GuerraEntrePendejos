using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [Header("Minigames")]
    [SerializeField] private List<GameObject> _minigames;

    // void OnEnable() => StartMinigame.OnStartNewMinigame += MinigameNumber;
    // void OnDisable() => StartMinigame.OnStartNewMinigame -= MinigameNumber;

    private void Start() => StartMinigame();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EndMinigame();
    }

    public void StartMinigame()
    {
        // if (_minigames.Count > 0)
        // {
        //     Debug.LogError($"Minigioco con indice {GameManager.Instance.MinigameIndexLocal} non esiste!");
        //     return;
        // }

        ChooseMinigame(GameManager.Instance.MinigameIndexLocal);
    }

    public void ChooseMinigame(int index)
    {
        if (index < 0 || index >= _minigames.Count) return;
        _minigames[index].SetActive(true);
    }

    public void EndMinigame()
    {
        if (_minigames.Count > 0)
            _minigames[GameManager.Instance.MinigameIndexLocal].SetActive(false);
    }
}
