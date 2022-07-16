using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public int health = 3;
  public float speed;
  public float jumpForce;
  public GameObject fireBall;
  public Transform firePoint;

  private Rigidbody2D rigidBody;
  private Animator animator;
  private bool isJumping;
  private bool isFiring;
  private const int GROUND_LAYER = 6;
  private enum AnimationStates
  {
    Idle,
    Run,
    Jump,
    Fire,
  }

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();

    UpdateLives();
  }

  void Update()
  {
    Jump();
    HandleFireBallCoroutine();
  }

  void FixedUpdate()
  {
    Move();
  }

  void UpdateLives()
  {
    GameController.instance.UpdateLives(health);
  }

  void SetTransition(AnimationStates state)
  {
    animator.SetInteger("transition", (int)state);
  }

  void Move()
  {
    float horizontalAxisIntensity = Input.GetAxis("Horizontal");

    rigidBody.velocity = new Vector2(horizontalAxisIntensity * speed, rigidBody.velocity.y);

    bool isGoingRight = horizontalAxisIntensity > 0;
    bool isGoingLeft = horizontalAxisIntensity < 0;
    bool isStopped = horizontalAxisIntensity == 0;
    bool isOnTransitionalState = isJumping || isFiring;

    if (isGoingRight)
    {
      if (!isOnTransitionalState)
      {
        SetTransition(AnimationStates.Run);
      }

      transform.eulerAngles = new Vector3(0, 0, 0);
    }
    else if (isGoingLeft)
    {
      if (!isOnTransitionalState)
      {
        SetTransition(AnimationStates.Run);
      }

      transform.eulerAngles = new Vector3(0, 180, 0);
    }
    else if (isStopped && !isOnTransitionalState)
    {
      SetTransition(AnimationStates.Idle);
    }
  }

  void Jump()
  {
    if (Input.GetButtonDown("Jump"))
    {
      if (!isJumping)
      {
        isJumping = true;
        SetTransition(AnimationStates.Jump);
        rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
      }
    }
  }

  void HandleFireBallCoroutine()
  {
    StartCoroutine("FireBall");
  }

  IEnumerator FireBall()
  {
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      isFiring = true;

      SetTransition(AnimationStates.Fire);

      GameObject fb = Instantiate(fireBall, firePoint.position, firePoint.rotation);

      bool isGoingRight = transform.rotation.y == 0;
      bool isGoingLeft = transform.rotation.y == 180;

      if (isGoingRight)
      {
        fb.GetComponent<FireBall>().isGoingRight = true;
      }

      if (isGoingLeft)
      {
        fb.GetComponent<FireBall>().isGoingRight = false;
      }

      yield return new WaitForSeconds(0.25f);

      isFiring = false;
    }
  }

  public void OnDamage(int damage)
  {
    health -= damage;
    UpdateLives();
    animator.SetTrigger("hit");

    if (transform.rotation.y >= 0)
    {
      transform.position += new Vector3(-0.5f, 0, 0);
    }
    else
    {
      transform.position += new Vector3(0.5f, 0, 0);
    }

    if (health <= 0)
    {
      // TODO: Game over
    }
  }

  public void AddHealth(int health)
  {
    this.health += health;
    UpdateLives();
  }

  void OnCollisionEnter2D(Collision2D collision2D)
  {
    if (collision2D.gameObject.layer == GROUND_LAYER)
    {
      isJumping = false;
    }
  }
}
