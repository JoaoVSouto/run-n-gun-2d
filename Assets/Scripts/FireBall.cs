using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
  public float speed;
  public bool isGoingRight;
  public int damage;

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

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Enemy")
    {
      collision.GetComponent<Goblin>().OnDamage(damage);
      Destroy(gameObject);
    }

    // if (collision.gameObject.tag == "Stone")
    // {
    //   Destroy(gameObject);
    // }

    if (collision.gameObject.layer == Player.GROUND_LAYER)
    {
      Destroy(gameObject);
    }
  }
}
