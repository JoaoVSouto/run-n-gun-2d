using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDispatcher
{
  void Invoke(Action fn);
}