using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Client
{
    public string id;
}

public class User
{
    public string id;
    public string name;
    public string admin;
    public string email;
    public string cpf;
    public string avatar_id;
    public Client client;
    public string role;
    public string avatar;
}

public class LoginBodyResponse
{
    public string access_token;
    public User user; 
}

public class LoginScreenBehaviour : MonoBehaviour
{
    public InputField _login, _password;
    Toggle _toggle;

    private void Start()
    {
        _toggle = GetComponentInChildren<Toggle>();

        if (PlayerPrefs.HasKey("RememberMe"))
        {
            _login.text = PlayerPrefs.GetString("RememberMe");
            _password.text = PlayerPrefs.GetString(_login.text + "Password");
            StartCoroutine(Upload());
        }
    }

    public void ConfirmButton()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("login", _login.text);
        form.AddField("password", _password.text);
        form.AddField("grant_type", "password");

        //Getting access token
        using (UnityWebRequest www = UnityWebRequest.Post("https://api.gamemind.com.br/oauth/token", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(www.downloadHandler.text);
                LoginBodyResponse body = JsonUtility.FromJson<LoginBodyResponse>(www.downloadHandler.text);
                if (_toggle.isOn) GameManager.instance.SaveCredentials(_login.text, _password.text);
                GameManager.instance.SetToken(body.access_token, _login.text);
                GameManager.instance.LoadScene("StageSelector");
            }
        }
    }

    
}
