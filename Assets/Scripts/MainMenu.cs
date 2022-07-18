using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
  public static int REGISTER_ACCOUT_INDEX = 2;
  public static int LEVEL_1_BUILD_INDEX = 1;
  public static int LOBBY_INDEX = 3;

  string username;
  string password;

  string jsonDataLoginForm;

  public void RegisterAccount()
  {
    SceneManager.LoadScene(REGISTER_ACCOUT_INDEX);
  }

  public void LoadGame()
  {
    SceneManager.LoadScene(LEVEL_1_BUILD_INDEX);
  }

  public void ReadUsernameInput(string s)
  {
    username = s;
  }

  public void ReadPasswordInput(string s)
  {
    password = s;
  }

  public void Login()
  {
    Dictionary<string, string> form = new Dictionary<string, string>{
        {"username", username},
        {"password", password}
    };

    var json = JsonConvert.SerializeObject(form);

    jsonDataLoginForm = json;

    StartCoroutine(LoginCoroutine());
  }

  IEnumerator LoginCoroutine()
  {
    string uri = "http://localhost:3000/login";

    using UnityWebRequest webRequest = UnityWebRequest.Post(uri, "POST");
    webRequest.SetRequestHeader("Content-Type", "application/json");
    byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonDataLoginForm);

    webRequest.uploadHandler = new UploadHandlerRaw(data);
    webRequest.downloadHandler = new DownloadHandlerBuffer();
    yield return webRequest.SendWebRequest();


    switch (webRequest.result)
    {
      case UnityWebRequest.Result.Success:
        Debug.Log("Success");
        string response = webRequest.downloadHandler.text;
        JObject jsonResponse = JObject.Parse(response);
        string accessToken = jsonResponse.GetValue("accessToken").ToString();
        GlobalData.accessToken = accessToken;
        GlobalData.username = username;
        SceneManager.LoadScene(LOBBY_INDEX);
        break;
      case UnityWebRequest.Result.ConnectionError:
        Debug.Log("Connection Error");
        break;
      case UnityWebRequest.Result.ProtocolError:
        Debug.Log("Protocol Error");
        break;
    }
  }
}
