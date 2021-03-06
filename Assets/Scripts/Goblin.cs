using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
  public float speed;
  public float walkTowardsDirectionTime;
  public int health;
  public int damage;

  private Rigidbody2D rigidBody;
  private Animator animator;
  private float timer;
  private bool isWalkingToRight;

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
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

  public void OnDamage(int damage)
  {
    health -= damage;
    animator.SetTrigger("hit");

    if (health <= 0)
    {
      // TODO: add death animation
      Destroy(gameObject);
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      collision.gameObject.GetComponent<Player>().OnDamage(damage);
    }
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      collision.gameObject.GetComponent<Player>().OnDamage(damage);
    }
  }
}
