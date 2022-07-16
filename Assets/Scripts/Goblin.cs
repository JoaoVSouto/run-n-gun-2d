using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
  public float speed;
  public float walkTowardsDirectionTime;

  private Rigidbody2D rigidBody;
  private float timer;
  private bool isWalkingToRight;

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate()
  {
    timer += Time.deltaTime;

    if (timer >= walkTowardsDirectionTime)
    {
      isWalkingToRight = !isWalkingToRight;
      timer = 0;
    }

    if (isWalkingToRight)
    {
      transform.eulerAngles = new Vector2(0, 0);
      rigidBody.velocity = Vector2.right * speed;
    }
    else
    {
      transform.eulerAngles = new Vector2(0, 180);
      rigidBody.velocity = Vector2.left * speed;
    }
  }
}
