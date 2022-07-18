using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatcherUpdate : MonoBehaviour
{
  void Update()
  {
    Dispatcher.Instance.InvokePending();
  }
}