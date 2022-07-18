using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dispatcher : IDispatcher
{
  public List<Action> pending = new List<Action>();


  private static Dispatcher instance;

  public static Dispatcher Instance
  {
    get
    {
      if (instance == null)
      {
        // Instance singleton on first use.
        instance = new Dispatcher();
      }

      return instance;
    }
  }

  public void Invoke(Action fn)
  {
    pending.Add(fn);
  }

  public void InvokePending()
  {
    try
    {
      foreach (var action in pending)
      {
        action(); // Invoke the action.
      }
    }
    catch (Exception e)
    {
      // Debug.Log(e);
    }
    finally
    {
      pending.Clear();
    }
  }
}