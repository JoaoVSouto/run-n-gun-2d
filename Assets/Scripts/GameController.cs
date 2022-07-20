using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  public Text healthText;
  public Text scoreText;
  public Text gameWinScoreText;
  public Text gameOverScoreText;
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
    string coinText = score == 1 ? "coin" : "coins";
    gameOverScoreText.text = "You got only " + score.ToString() + " " + coinText;
  }

  public void GameWon()
  {
    isGameOver = true;
    gameWinObject.SetActive(true);
    string coinText = score == 1 ? "coin" : "coins";
    gameWinScoreText.text = "You got " + score.ToString() + " " + coinText;
  }

  public void Restart()
  {
    SceneManager.LoadScene(LOBBY_BUILD_INDEX);
  }
}
