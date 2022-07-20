using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
  public float smooth;
  public int damage;

  private Rigidbody2D rigidBody;
  private Transform player;

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    rigidBody = GetComponent<Rigidbody2D>();
    Destroy(gameObject, 3f);
  }

  void FixedUpdate()
  {
    Vector3 playerPosition = new Vector3(player.position.x, player.position.y + 1, transform.position.z);
    transform.position = Vector3.Lerp(transform.position, playerPosition, smooth * Time.deltaTime);
    transform.Rotate(0, 0, 240 * Time.deltaTime);
  }

  void OnTriggerEnter2D(Collider2D collision2D)
  {
    if (collision2D.gameObject.tag == "Player")
    {
      collision2D.GetComponent<Player>().OnDamage(damage);
      Destroy(gameObject);
    }

    if (collision2D.gameObject.tag == "FireBall")
    {
      Destroy(gameObject);
    }

    if (collision2D.gameObject.layer == Player.GROUND_LAYER)
    {
      Destroy(gameObject);
    }
  }
}
