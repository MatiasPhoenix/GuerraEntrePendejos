using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RingMinigameManager : MonoBehaviour
{
    public static RingMinigameManager Instance;
    [SerializeField] private List<GameObject> _ringList = new List<GameObject>();
    [SerializeField] private float _timerNumber;
    [SerializeField] private GameObject _buttonObject;
    [SerializeField] private GameObject _timeFinishedGameObject;
    [SerializeField] private TextMeshPro _timeFinished;
    [SerializeField] private TextMeshPro _timer;

    bool _ringGameOver = false;


    private GameObject _ringActive = null;

    private void Awake() => Instance = this;

    private void Start()
    {
        _ringActive = _ringList[0];
        _ringActive.SetActive(true);
    }
    private void Update()
    {
        TimerUpdate();
    }

    public void NextRing()
    {
        if (_ringActive == _ringList[2] && _ringGameOver == false)
        {
            _ringActive.SetActive(false);
            _ringActive = _ringList[0];
            _ringActive.SetActive(true);
        }
        else if (_ringGameOver == false)
        {
            _ringActive.SetActive(false);
            _ringActive = _ringList[_ringList.IndexOf(_ringActive) + 1];
            _ringActive.SetActive(true);
        }
    }

    void TimerUpdate()
    {
        if (_timerNumber <= 0)
        {
            _ringActive.SetActive(false);
            ButtonInteractionActivation();
            return;
        }
        _timerNumber -= Time.deltaTime;
        _timer.text = $"Timer: {(int)_timerNumber}";
    }
    void ButtonInteractionActivation()
    {
        _ringGameOver = true;
        _buttonObject.SetActive(true);
        _timeFinishedGameObject.SetActive(true);
        _timeFinished.text = $"Time finished!";
    }
}
