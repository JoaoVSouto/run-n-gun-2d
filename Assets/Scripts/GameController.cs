using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  public Text healthText;
  public Text scoreText;
  public GameObject gameOverObject;

  private int score;
  private bool isGameOver;

  public static GameController instance;

  private static int MAIN_MENU_BUILD_INDEX = 0;

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
    if (!isGameOver)
    {
      healthText.text = "x " + value.ToString();
    }
  }

  public void GameOver()
  {
    isGameOver = true;
    gameOverObject.SetActive(true);
  }

  public void Restart()
  {
    SceneManager.LoadScene(MAIN_MENU_BUILD_INDEX);
  }
}
