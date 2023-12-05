using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestWrapper : MonoBehaviour
{
    public static UnityWebRequest Post(string url, object data)
    {
        var str = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(str);

        UnityWebRequest request = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(bodyRaw)
        };

        request.SetRequestHeader("Authorization", "Bearer " + GameManager.instance.accessToken);
        request.SetRequestHeader("accept", "*/*");
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    public static UnityWebRequest Put(string url, object data)
    {
        var str = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(str);

        UnityWebRequest request = new UnityWebRequest(url, "PUT")
        {
            uploadHandler = new UploadHandlerRaw(bodyRaw)
        };

        request.SetRequestHeader("Authorization", "Bearer " + GameManager.instance.accessToken);
        request.SetRequestHeader("accept", "*/*");
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }
}
