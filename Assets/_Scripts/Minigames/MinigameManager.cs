using System;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [Header("Minigames")]
    [SerializeField] private List<GameObject> _minigames;

    private int _minigamesIndex;
    public event EventHandler MinigamesEndEvent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EndMinigame(_minigamesIndex);
    }

    public void StartMinigame(int index) => _minigames[index].SetActive(true);

    public void EndMinigame(int index) => _minigames[index].SetActive(false);


}
