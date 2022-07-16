using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public Text healthText;
  public static GameController instance;

  void Awake()
  {
    instance = this;
  }

  public void UpdateLives(int value)
  {
    healthText.text = "x " + value.ToString();
  }
}
