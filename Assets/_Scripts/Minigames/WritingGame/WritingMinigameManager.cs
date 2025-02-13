using TMPro;
using UnityEngine;

public class WritingMinigameManager : MonoBehaviour
{
    [Header("Minigame elements")]
    [SerializeField] private TextMeshPro _writeObject;
    [SerializeField] private GameObject _winObject;
    [SerializeField] private GameObject _loseObject;


    private char _currentLetter;
    private bool _inputPressed = false;

    void Start() => SetNewLetter();

    void Update()
    {
        if (Input.inputString.Length > 0 && !_inputPressed)
        {
            char pressedLetter = Input.inputString[0];

            if (pressedLetter == _currentLetter)
            {
                _winObject.SetActive(true);
                _inputPressed = true; 
            }
            else
            {
                _loseObject.SetActive(true);
                _inputPressed = true; 
            }
            Invoke("SetNewLetter", 0.5f); // Invoca la funzione 
        }
    }

    void SetNewLetter()
    {
        if (!_writeObject.gameObject.activeSelf)
            _writeObject.gameObject.SetActive(true);

        _currentLetter = "abcdefghijklmnopqrstuvwxyz"[Random.Range(0, 26)];
        _writeObject.text = _currentLetter.ToString();
        _writeObject.transform.position = RandomPosition();
        _writeObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-20, 20));
        _winObject.SetActive(false);
        _loseObject.SetActive(false); 
        _inputPressed = false;
    }

    Vector2 RandomPosition() => new Vector2(Random.Range(-2f, 2.5f), Random.Range(-2f, 2f));

}
