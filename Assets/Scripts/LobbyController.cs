using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LobbyController : MonoBehaviour
{
  public TextMeshProUGUI usernameText;
  void Start()
  {
    usernameText.text = GlobalData.username;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
