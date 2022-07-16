using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
  public int scoreAddCount;

  bool hasTriggered = false;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Player" && !hasTriggered)
    {
      hasTriggered = true;
      GameController.instance.AddScore(scoreAddCount);
      Destroy(gameObject);
    }
  }
}
