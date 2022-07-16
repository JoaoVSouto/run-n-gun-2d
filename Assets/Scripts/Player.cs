using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public float speed;
  public float jumpForce;

  private Rigidbody2D rigidBody;
  private Animator animator;

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }

  void Update()
  {
    Move();
  }

  void Move()
  {
    float horizontalAxisIntensity = Input.GetAxis("Horizontal");

    rigidBody.velocity = new Vector2(horizontalAxisIntensity * speed, rigidBody.velocity.y);

    bool isGoingRight = horizontalAxisIntensity > 0;
    bool isGoingLeft = horizontalAxisIntensity < 0;

    if (isGoingRight)
    {
      transform.eulerAngles = new Vector3(0, 0, 0);
    }
    else if (isGoingLeft)
    {
      transform.eulerAngles = new Vector3(0, 180, 0);
    }
  }

  void Jump()
  {

  }
}
