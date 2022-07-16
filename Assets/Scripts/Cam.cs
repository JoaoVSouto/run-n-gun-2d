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

  void LateUpdate()
  {
    if (player != null && player.position.x >= 0 && player.position.x <= 18)
    {
      Vector3 playerPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
      transform.position = Vector3.Lerp(transform.position, playerPosition, smooth * Time.deltaTime);
    }
  }
}
