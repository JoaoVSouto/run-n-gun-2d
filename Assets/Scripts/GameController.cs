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
  public GameObject gameWinObject;

  private int score;
  private bool isGameOver;
  private bool isGameWon;

  public static GameController instance;

  private static int LOBBY_BUILD_INDEX = 3;

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
    if (!isGameOver && !isGameWon)
    {
      healthText.text = "x " + value.ToString();
    }
  }

  public void GameOver()
  {
    isGameOver = true;
    gameOverObject.SetActive(true);
  }

  public void GameWon()
  {
    isGameOver = true;
    gameWinObject.SetActive(true);
  }

  public void Restart()
  {
    SceneManager.LoadScene(LOBBY_BUILD_INDEX);
  }
}
