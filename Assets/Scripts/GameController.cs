using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  public Text healthText;
  public Text scoreText;

  private int score;

  public static GameController instance;

  void Awake()
  {
    instance = this;
  }
  public void AddScore(int score)
  {
    this.score += score;
    scoreText.text = this.score.ToString();
  }

  public void UpdateLives(int value)
  {
    healthText.text = "x " + value.ToString();
  }
}
