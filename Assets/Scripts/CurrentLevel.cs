using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CurrentLevel
{
  private int currentLevel = 1;

  private CurrentLevel() { }

  private static CurrentLevel instance = null;

  public static CurrentLevel Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new CurrentLevel();
      }
      return instance;
    }
  }

  public void setCurrentLevel(int level)
  {
    this.currentLevel = level;
  }

  public int getCurrentLevel()
  {
    return this.currentLevel;
  }
}
