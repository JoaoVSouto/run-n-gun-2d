using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
  public float speed;
  public bool isGoingRight;

  private Rigidbody2D rigidBody;

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    Destroy(gameObject, 2f);
  }

  void FixedUpdate()
  {
    if (isGoingRight)
    {
      rigidBody.velocity = Vector2.right * speed;
    }
    else
    {
      rigidBody.velocity = Vector2.left * speed;
    }
  }
}
