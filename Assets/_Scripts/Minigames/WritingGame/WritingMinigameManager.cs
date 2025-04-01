using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WritingMinigameManager : MonoBehaviour
{
    [Header("Minigame elements")]
    [SerializeField] private TextMeshPro _writeObject;
    [SerializeField] private GameObject _winObject;
    [SerializeField] private GameObject _loseObject;
    public GameObject _buttonObject;

    [Header("Counter & Timer")]
    [SerializeField] private TextMeshPro _timer;
    [SerializeField] private float _timerNumber;
    [SerializeField] private TextMeshPro _successText;
    [SerializeField] private TextMeshPro _missText;
    [SerializeField] private GameObject _timeFinishedGameObject;
    [SerializeField] private TextMeshPro _timeFinished;

    private char _currentLetter;
    private int _currentLetterNumber = 0;
    private bool _inputPressed = false;
    private string _originalText;

    private int _successPoint = 0;
    private int _failPoint = 0;

    void Start()
    {
        _originalText = _writeObject.text;
        SetNewLetter();
    }

    void Update()
    {
        if (Input.inputString.Length > 0 && !_inputPressed)
        {
            char pressedLetter = Input.inputString[0];

            if (pressedLetter == _currentLetter && _timerNumber >= 0)
            {
                _winObject.SetActive(true);
                _inputPressed = true;

                if (pressedLetter != ' ' || pressedLetter != '.' || pressedLetter != ',' || pressedLetter != '!' || pressedLetter != '?')
                    GameManager.Instance.WarScoreCounter(1);

                _successPoint++;
            }
            else if (pressedLetter != _currentLetter && _timerNumber >= 0)
            {
                _loseObject.SetActive(true);
                _inputPressed = true;

                _failPoint++;
            }else
            {
                return;
            }
            if (_timerNumber >= 0)
            {
                PointUpdate();
                Invoke(nameof(SetNewLetter), 0.05f);  // Tempo aumentato per visualizzare il risultato
            }
        }

        TimerUpdate();
    }

    void SetNewLetter()
    {
        if (!_writeObject.gameObject.activeSelf)
            _writeObject.gameObject.SetActive(true);

        if (_currentLetterNumber >= _originalText.Length)
        {
            Debug.Log("Hai finito il testo!");
            return;
        }

        // Ottieni la lettera corrente
        _currentLetter = _originalText[_currentLetterNumber];

        // Ricostruisci il testo formattato
        string coloredText = "";
        for (int i = 0; i < _originalText.Length; i++)
        {
            if (i == _currentLetterNumber)
                coloredText += $"<color=green>{_originalText[i]}</color>";
            else
                coloredText += _originalText[i];
        }

        // Assegna il testo formattato a TextMeshPro
        _writeObject.text = coloredText;

        _currentLetterNumber++;

        _winObject.SetActive(false);
        _loseObject.SetActive(false);
        _inputPressed = false;
    }

    void PointUpdate()
    {
        _successText.text = $"Success: {_successPoint}";
        _missText.text = $"Miss: {_failPoint}";
    }
    void TimerUpdate()
    {
        if (_timerNumber <= 0)
        {
            ButtonInteractionActivation();
            return;
        }
        _timerNumber -= Time.deltaTime;
        _timer.text = $"Timer: {(int)_timerNumber}";
    }
    void ButtonInteractionActivation()
    {
        _buttonObject.SetActive(true);
        _timeFinishedGameObject.SetActive(true);
        _timeFinished.text = $"Time finished!";
    }

}
