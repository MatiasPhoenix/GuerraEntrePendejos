using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  //Movimento e Rigidbody
  public InputAction MoveAction;
  public float MoveSpeed = 4;
  Rigidbody2D rigidbody2d;
  Vector2 move;

  Animator animator;

  Vector2 moveDirection = new Vector2(1, 0);
  public GameObject projectilePrefab;

  AudioSource audioSource;


  void Start()
  {
    MoveAction.Enable();
    rigidbody2d = GetComponent<Rigidbody2D>();

    animator = GetComponent<Animator>();

  }

  void Update()
  {
    move = MoveAction.ReadValue<Vector2>();

    if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
    {
      moveDirection.Set(move.x, move.y);
      moveDirection.Normalize();
    }
    animator.SetFloat("Look X", moveDirection.x);
    animator.SetFloat("Look Y", moveDirection.y);
    animator.SetFloat("Speed", move.magnitude);
  }

  void FixedUpdate()
  {
    Vector2 position = (Vector2)rigidbody2d.position + move * MoveSpeed * Time.deltaTime;
    rigidbody2d.MovePosition(position);
  }
  
  public void PlaySound(AudioClip clip)
  {
    audioSource.PlayOneShot(clip);
  }
}
