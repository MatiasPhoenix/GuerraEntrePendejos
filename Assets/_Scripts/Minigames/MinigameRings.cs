using UnityEngine;

public class MinigameRings : MonoBehaviour
{
    public Transform baseRing;     
    public Transform shrinkingRing;
    public Transform internalSpace;

    public float _shrinkSpeed = 1f; 
    private float _originalScale;   
    private bool _hasPressed = false; 
    private bool _success = false;    

    void Start()
    {
        _originalScale = shrinkingRing.localScale.x;

        Rigidbody2D rbShrinking = shrinkingRing.GetComponent<Rigidbody2D>();
        if (rbShrinking == null)
        {
            rbShrinking = shrinkingRing.gameObject.AddComponent<Rigidbody2D>();
        }
        rbShrinking.bodyType = RigidbodyType2D.Kinematic; 
        shrinkingRing.GetComponent<CircleCollider2D>().isTrigger = true;

        Rigidbody2D rbBase = baseRing.GetComponent<Rigidbody2D>();
        if (rbBase == null)
        {
            rbBase = baseRing.gameObject.AddComponent<Rigidbody2D>();
        }
        rbBase.bodyType = RigidbodyType2D.Kinematic;

        if (internalSpace != null)
        {
            Rigidbody2D rbInternal = internalSpace.GetComponent<Rigidbody2D>();
            if (rbInternal == null)
            {
                rbInternal = internalSpace.gameObject.AddComponent<Rigidbody2D>();
            }
            rbInternal.bodyType = RigidbodyType2D.Kinematic; 
        }

        Debug.Log("Gioco iniziato, collider configurati.");
    }

    void Update()
    {
        if (_hasPressed) return;

        shrinkingRing.localScale -= Vector3.one * _shrinkSpeed * Time.deltaTime * 0.3f;

        if (shrinkingRing.localScale.x < 0.005f)
        {
            ResetGame();
        }

        float shrinkingRadius = shrinkingRing.GetComponent<CircleCollider2D>().radius * shrinkingRing.localScale.x;
        float baseRadius = baseRing.GetComponent<CircleCollider2D>().radius * baseRing.localScale.x;
        float internalRadius = 0f;

        if (internalSpace != null)
        {
            internalRadius = internalSpace.GetComponent<CircleCollider2D>().radius * internalSpace.localScale.x;
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
        if (shrinkingRadius <= baseRadius && !_success && (internalSpace == null || shrinkingRadius > internalRadius))
        {
            if (_success) return;
            _success = true;
            Debug.Log("Azione riuscita! I cerchi sono sovrapposti.");
            
        }
        else 
        {
            Debug.Log("Hai fallito! Non hai premuto SPAZIO al momento giusto.");
            Invoke(nameof(ResetGame), 1f);
            _hasPressed = true;
            _success = false;
        }
    }

    void ResetGame()
    {
        _hasPressed = false;
        _success = false;
        shrinkingRing.localScale = Vector3.one * _originalScale;
    }
}