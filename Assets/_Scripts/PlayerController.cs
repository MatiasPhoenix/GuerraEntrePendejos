using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Objects")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private BaseUnit thisBaseUnit;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    void Start() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)) AnimatorControll();
        
            
        
        // Input movimento
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // Evita velocità maggiore in diagonale

        if (moveInput.x > 0)
            transform.localScale = new Vector3(-1, 1, 1); // Guarda a sinistra
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(1, 1, 1);  // Guarda a destra


        // Se il personaggio si muove, attiva animazione, altrimenti la ferma
        if (AnimationManager.Instance != null) // Controlla se il singleton esiste
        {

            bool isWalking = moveInput != Vector2.zero;
            AnimationManager.Instance.PlayWalkAnimation(thisBaseUnit, isWalking);
        }
        rb.linearVelocity = moveInput * speed;
    }

    void AnimatorControll()
    {
        if (AnimationManager.Instance != null)
            Debug.LogWarning($"Controlliamo è null?: {AnimationManager.Instance == null}");
        else
            Debug.LogWarning($"StoCazzoDiAnimatorManager è null?: {AnimationManager.Instance == null}");
            
    }

}
