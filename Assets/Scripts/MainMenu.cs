using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public static int LEVEL_1_BUILD_INDEX = 1;

  public void LoadGame()
  {
    SceneManager.LoadScene(LEVEL_1_BUILD_INDEX);
  }
}
