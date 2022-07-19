using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
  private Transform player;
  public float smooth;

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;
  }

  float getCameraYByLevel()
  {
    switch (CurrentLevel.Instance.getCurrentLevel())
    {
      case 1:
        return 0;
      case 2:
      default:
        return -13.24f;
    }
  }

  bool getPlayerPositionByLevel()
  {
    switch (CurrentLevel.Instance.getCurrentLevel())
    {
      case 1:
        return player.position.x >= -5 && player.position.x <= 120;
      case 2:
      default:
        return player.position.x >= -5 && player.position.x <= 20;
    }
  }

  void LateUpdate()
  {
    if (player != null && getPlayerPositionByLevel())
    {
      Vector3 playerPosition = new Vector3(player.position.x, getCameraYByLevel(), transform.position.z);
      transform.position = Vector3.Lerp(transform.position, playerPosition, smooth * Time.deltaTime);
    }
  }
}
