using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
  public GameObject stone;
  public Transform firePoint;
  public float timeBetweenThrowsInSeconds;

  private bool hasFired;

  void FixedUpdate()
  {
    HandleFireStoneCoroutine();
  }

  void HandleFireStoneCoroutine()
  {
    StartCoroutine("FireStone");
  }

  IEnumerator FireStone()
  {
    if (!hasFired)
    {
      hasFired = true;
      Instantiate(stone, firePoint.position, firePoint.rotation);

      yield return new WaitForSeconds(timeBetweenThrowsInSeconds);

      hasFired = false;
    }
  }
}
