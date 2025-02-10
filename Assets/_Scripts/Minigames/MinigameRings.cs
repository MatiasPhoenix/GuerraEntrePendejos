using UnityEngine;

public class MinigameRings : MonoBehaviour
{
   public Transform baseRing;      // L'anello fisso
    public Transform shrinkingRing;  // L'anello che si rimpicciolisce

    public float shrinkSpeed = 1f;   // Velocità di riduzione
    private float originalScale;    // Scala iniziale
    private bool hasPressed = false; // Controllo se il giocatore ha premuto
    private bool success = false;    // Indica se il giocatore ha avuto successo

    void Start()
    {
        originalScale = shrinkingRing.localScale.x;

        // Assicurati che shrinkingRing abbia un Rigidbody2D e sia un Trigger
        Rigidbody2D rbShrinking = shrinkingRing.GetComponent<Rigidbody2D>();
        if (rbShrinking == null)
        {
            rbShrinking = shrinkingRing.gameObject.AddComponent<Rigidbody2D>();
            rbShrinking.isKinematic = true; // Impostalo su Kinematic per non influenzare la fisica
        }
        shrinkingRing.GetComponent<CircleCollider2D>().isTrigger = true;

        // Assicurati che baseRing abbia un Rigidbody2D
        Rigidbody2D rbBase = baseRing.GetComponent<Rigidbody2D>();
        if (rbBase == null)
        {
            rbBase = baseRing.gameObject.AddComponent<Rigidbody2D>();
            rbBase.isKinematic = true; // Impostalo su Kinematic per non influenzare la fisica
        }

        Debug.Log("Gioco iniziato, collider configurati.");
    }

    void Update()
    {
        if (hasPressed) return; // Se il giocatore ha già premuto, non fare nulla

        // Rimpicciolisce l'anello
        shrinkingRing.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime * 0.3f;

        // Se il cerchio è diventato troppo piccolo, fallimento automatico
        if (shrinkingRing.localScale.x < 0.005f)
        {
            Debug.Log("Hai fallito! Il cerchio è scomparso.");
            ResetGame();
        }

        // Se il giocatore preme SPAZIO
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }

        // Controllo del successo basato sui raggi
        float shrinkingRadius = shrinkingRing.GetComponent<CircleCollider2D>().radius * shrinkingRing.localScale.x;
        float baseRadius = baseRing.GetComponent<CircleCollider2D>().radius * baseRing.localScale.x;

        if (shrinkingRadius <= baseRadius && !success)
        {
            Success();
        }
        else if (shrinkingRadius > baseRadius && success)
        {
            //Se il giocatore ha premuto spazio mentre era dentro, e poi il cerchio è uscito, deve fallire
            if (hasPressed)
            {
                Debug.Log("Hai fallito! Non hai premuto SPAZIO al momento giusto.");
                Invoke(nameof(ResetGame), 1f);
                hasPressed = true;
                success = false;
            }
        }

    }

    void CheckSuccess()
    {
        hasPressed = true;
        if (!success)
        {
            Debug.Log("Hai fallito! Non hai premuto SPAZIO al momento giusto.");
        }
        Invoke(nameof(ResetGame), 1f);
    }

    void Success()
    {
        if (success) return; // Evita di chiamare Success più volte
        success = true;
        Debug.Log("Azione riuscita! I cerchi sono sovrapposti.");
    }

    void ResetGame()
    {
        shrinkingRing.localScale = Vector3.one * originalScale;
        hasPressed = false;
        success = false;
    }
}
