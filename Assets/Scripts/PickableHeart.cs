using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableHeart : MonoBehaviour
{
  public int healthAddCount;

  bool hasTriggered = false;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Player" && !hasTriggered)
    {
      hasTriggered = true;
      collision.gameObject.GetComponent<Player>().AddHealth(healthAddCount);
      Destroy(gameObject);
    }
  }
}
