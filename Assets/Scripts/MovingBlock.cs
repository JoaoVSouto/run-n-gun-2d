using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
  public float walkTowardsDirectionTime;
  public float speed;

  public bool isGoingRight;
  private float timer;
  private Rigidbody2D rigidBody;

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate()
  {
    timer += Time.deltaTime;

    if (timer >= walkTowardsDirectionTime)
    {
      isGoingRight = !isGoingRight;
      timer = 0;
    }

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
