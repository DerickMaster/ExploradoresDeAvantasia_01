using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TokenMenuBehaviour : MonoBehaviour
{
    InputField _input;

    private void Start()
    {
        _input = GetComponentInChildren<InputField>();
    }

    public void ConfirmPressed()
    {
        if (_input.text != null)
        {
            WWWForm form = new WWWForm();
            form.AddField("myField", "testing");

            StartCoroutine(Upload(form));
        }
    }

    IEnumerator Upload(WWWForm form)
    {
        using UnityWebRequest www = UnityWebRequest.Get("https://gamemind-api.herokuapp.com/admin/games/1/save");
        www.SetRequestHeader("Authorization", "Bearer " + _input.text);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.result);
        }
    }
}