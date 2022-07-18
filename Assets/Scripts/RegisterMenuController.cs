using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class RegisterMenuController : MonoBehaviour
{
  string name_register;
  string username;
  string email;
  string password;

  string jsonDataRegisterForm;

  public static int MENU_SCENE_INDEX = 0;

  public void ReadNameInput(string s)
  {
    name_register = s;
  }

  public void ReadUsernameInput(string s)
  {
    username = s;
  }

  public void ReadEmailInput(string s)
  {
    email = s;
  }

  public void ReadPasswordInput(string s)
  {
    password = s;
  }

  public void BackToMenu()
  {
    SceneManager.LoadScene(MENU_SCENE_INDEX);
  }

  public void RegisterAccount()
  {
    Dictionary<string, string> form = new Dictionary<string, string>{
        {"name", name_register},
        {"username", username},
        {"email", email},
        {"password", password}
    };

    var json = JsonConvert.SerializeObject(form);

    jsonDataRegisterForm = json;

    StartCoroutine(RegisterAccountCoroutine());
  }

  IEnumerator RegisterAccountCoroutine()
  {
    string uri = "http://localhost:3000/users";

    using UnityWebRequest webRequest = UnityWebRequest.Post(uri, "POST");
    webRequest.SetRequestHeader("Content-Type", "application/json");
    byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonDataRegisterForm);

    webRequest.uploadHandler = new UploadHandlerRaw(data);
    webRequest.downloadHandler = new DownloadHandlerBuffer();
    yield return webRequest.SendWebRequest();

    if (webRequest.isNetworkError)
    {
      Debug.Log("Error: " + webRequest.error);
    }
    else
    {
      Debug.Log("Register Success");
      SceneManager.LoadScene(MENU_SCENE_INDEX);
    }
  }
}
