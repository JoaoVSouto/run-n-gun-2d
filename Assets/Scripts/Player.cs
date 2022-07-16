using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public float speed;
  public float jumpForce;

  private Rigidbody2D rigidBody;
  private Animator animator;
  private bool isJumping;
  private const int GROUND_LAYER = 6;
  private enum AnimationStates
  {
    Idle,
    Run,
    Jump,
  }

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }

  void Update()
  {
    Move();
    Jump();
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
    bool isOnTransitionalState = isJumping;

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

  void OnCollisionEnter2D(Collision2D collision2D)
  {
    if (collision2D.gameObject.layer == GROUND_LAYER)
    {
      isJumping = false;
    }
  }
}
