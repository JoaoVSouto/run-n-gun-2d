using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class LobbyController : MonoBehaviour
{
  public static int LEVEL_1_BUILD_INDEX = 1;
  public TextMeshProUGUI usernameText;
  public GameObject startGameButton;

  WebSocket ws;

  void Start()
  {
    startGameButton.SetActive(false);
    ws = new WebSocket("ws://localhost:3000");
    usernameText.text = GlobalData.username;
    ws.Connect();
    ws.OnMessage += (sender, e) =>
    {
      // Debug.Log(e.Data);
      string data = e.Data;
      JObject json = JObject.Parse(data);
      string type = json.GetValue("type").ToString();
      if (type == "GAME_CREATED")
      {
        string gameId = json.GetValue("gameId").ToString();
        GlobalData.gameId = gameId;
        print(gameId);
      }
      if (type == "GAME_JOINED")
      {
        string gameId = json.GetValue("gameId").ToString();
        GlobalData.gameId = gameId;
        print(gameId);
      }
      if (type == "GAME_STARTED")
      {
        Dispatcher.Instance.Invoke(() =>
        {
          SceneManager.LoadScene(LEVEL_1_BUILD_INDEX);
        });
      }
      if (type == "GAME_STATS")
      {
        string gameData = json.GetValue("game").ToString();
        JObject gameJson = JObject.Parse(gameData);
        string players = gameJson.GetValue("players").ToString();
        JArray playersJson = JArray.Parse(players);
        List<PlayerServerData> playerList = playersJson.ToObject<List<PlayerServerData>>();
        if (playerList.Count == 2 && GlobalData.isHost)
        {
          Dispatcher.Instance.Invoke(() =>
        {
          ActiveStartButton();
        });
        }
      }

    };
    ws.Send("{\"type\":\"GET_LOBBY\"}");
  }

  void ActiveStartButton()
  {
    startGameButton.SetActive(true);
  }

  public void CreateGame()
  {
    ws.Send("{\"type\":\"CREATE_GAME\", \"accessToken\":\"" + GlobalData.accessToken + "\"}");
    GlobalData.isHost = true;
  }

  public void JoinGame()
  {
    ws.Send("{\"type\":\"JOIN_RANDOM_GAME\", \"accessToken\":\"" + GlobalData.accessToken + "\"}");
    GlobalData.isHost = false;
  }

  public void StartGame()
  {
    SceneManager.LoadScene(LEVEL_1_BUILD_INDEX);
    ws.Send("{\"type\":\"START_GAME\", \"accessToken\":\"" + GlobalData.accessToken + "\", \"gameId\":\"" + GlobalData.gameId + "\"}");
  }

  // Update is called once per frame
  void Update()
  {

  }
}
