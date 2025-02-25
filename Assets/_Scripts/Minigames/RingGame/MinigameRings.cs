using TMPro;
using UnityEngine;

public class MinigameRings : MonoBehaviour
{
    
    [Header("Rings objects")]
    [SerializeField] private Transform _baseRing;     
    [SerializeField] private Transform _internalSpace;
    [SerializeField] private Transform _shrinkingRing;
    [SerializeField] private float _shrinkSpeed = 1f; 

    [Header ("Text Ring Game")]
    [SerializeField] private TextMeshPro _winText;
    [SerializeField] private TextMeshPro _loseText;

    private float _originalScale;   
    private bool _hasPressed = false; 
    private bool _success = false;    

    void Start()
    {
        _originalScale = _shrinkingRing.localScale.x;

        Rigidbody2D rbShrinking = _shrinkingRing.GetComponent<Rigidbody2D>();
        if (rbShrinking == null)
        {
            rbShrinking = _shrinkingRing.gameObject.AddComponent<Rigidbody2D>();
        }
        rbShrinking.bodyType = RigidbodyType2D.Kinematic; 
        _shrinkingRing.GetComponent<CircleCollider2D>().isTrigger = true;

        Rigidbody2D rbBase = _baseRing.GetComponent<Rigidbody2D>();
        if (rbBase == null)
        {
            rbBase = _baseRing.gameObject.AddComponent<Rigidbody2D>();
        }
        rbBase.bodyType = RigidbodyType2D.Kinematic;

        if (_internalSpace != null)
        {
            Rigidbody2D rbInternal = _internalSpace.GetComponent<Rigidbody2D>();
            if (rbInternal == null)
            {
                rbInternal = _internalSpace.gameObject.AddComponent<Rigidbody2D>();
            }
            rbInternal.bodyType = RigidbodyType2D.Kinematic; 
        }

        Debug.Log("Gioco iniziato, collider configurati.");
    }

    void Update()
    {
        if (_hasPressed) return;

        _shrinkingRing.localScale -= Vector3.one * _shrinkSpeed * Time.deltaTime * 0.3f;

        if (_shrinkingRing.localScale.x < 0.005f)
        {
            ResetGame();
        }

        float shrinkingRadius = _shrinkingRing.GetComponent<CircleCollider2D>().radius * _shrinkingRing.localScale.x;
        float baseRadius = _baseRing.GetComponent<CircleCollider2D>().radius * _baseRing.localScale.x;
        float internalRadius = 0f;

        if (_internalSpace != null)
        {
            internalRadius = _internalSpace.GetComponent<CircleCollider2D>().radius * _internalSpace.localScale.x;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess(shrinkingRadius, baseRadius, internalRadius);
        }
    }

    void CheckSuccess(float shrinkingRadius, float baseRadius, float internalRadius)
    {
        _hasPressed = true;

        if (_hasPressed)
            Success(shrinkingRadius, baseRadius, internalRadius);

        Invoke(nameof(ResetGame), 1f);
    }

    void Success(float shrinkingRadius, float baseRadius, float internalRadius)
    {
        if (shrinkingRadius <= baseRadius && !_success && (_internalSpace == null || shrinkingRadius > internalRadius))
        {
            if (_success) return;
            _success = true;
            Debug.Log("Azione riuscita! I cerchi sono sovrapposti.");
            WinOrLose(true);
            GameManager.Instance.WarScoreCounter(10);
        }
        else 
        {
            Debug.Log("Hai fallito! Non hai premuto SPAZIO al momento giusto.");
            WinOrLose(false);
            // Invoke(nameof(ResetGame), 1f);
            _hasPressed = true;
            _success = false;
        }
    }

    void ResetGame()
    {
        _hasPressed = false;
        _success = false;
        RestartResult();
        _shrinkingRing.localScale = Vector3.one * _originalScale;
        RingMinigameManager.Instance.NextRing();
    }

    void WinOrLose(bool result)
    {
        if(result)
        _winText.gameObject.SetActive(true);
        else
        _loseText.gameObject.SetActive(true);
    }
    void RestartResult()
    {
        _winText.gameObject.SetActive(false);
        _loseText.gameObject.SetActive(false);
    }
}