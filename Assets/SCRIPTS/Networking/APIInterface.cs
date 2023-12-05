using System.Collections;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public interface IReceiveSave
{
    public void GetSaveData(AdventureManager.AdventureSaveData data);
}

public static class APIInterface
{
    [Serializable]
    struct dataArray
    {
        public record[] records;
    }

    [Serializable]
    struct data
    {
        public record record;
    }

    [Serializable]
    struct record
    {
        public int playtime;
        public bool finished;
        public string[] mistakes;
        public int stars;
        public int checkpoint;
        public int coin;
    }

    private static UnityWebRequest CreateWebRequest(string url, string requestType, string json = "")
    {
        byte[] rawData = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, requestType)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        if(json.Length >= 1) request.uploadHandler = new UploadHandlerRaw(rawData);

        request.SetRequestHeader("Authorization", "Bearer " + GameManager.instance.accessToken);
        request.SetRequestHeader("accept", "*/*");
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    public static IEnumerator SendPostRequest(string sceneSlug, AdventureManager.AdventureSaveData savedata)
    {
        string saveUrl = "https://api.gamemind.com.br/" + GameManager.instance.accountType + "/games/" + sceneSlug + "/save";
        string json = JsonUtility.ToJson(GenerateDataToSend(savedata));
        UnityWebRequest request = CreateWebRequest(saveUrl, "POST", json);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Could not post the save");
            Debug.Log(request.error);
            Debug.Log(Encoding.UTF8.GetString(request.downloadHandler.data));
        }
        else
        {
            Debug.Log("Save posted");
            Debug.Log(Encoding.UTF8.GetString(request.uploadHandler.data));
        }
    }

    public static IEnumerator SendPutRequest(string sceneSlug, AdventureManager.AdventureSaveData savedata)
    {
        string saveUrl = "https://api.gamemind.com.br/" + GameManager.instance.accountType + "/games/" + sceneSlug + "/save";
        string json = JsonUtility.ToJson(GenerateDataToSend(savedata));
        UnityWebRequest request = CreateWebRequest(saveUrl, "PUT", json);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Could not put the save");
            Debug.Log(request.error);
            Debug.Log(Encoding.UTF8.GetString(request.downloadHandler.data));
        }
        else
        {
            Debug.Log("Save updated");
            Debug.Log(Encoding.UTF8.GetString(request.uploadHandler.data));
        }
    }

    public static IEnumerator GetSaveRequest(string sceneSlug, IReceiveSave objToReceive)
    {
        string saveUrl = "https://api.gamemind.com.br/" + GameManager.instance.accountType + "/games/" + sceneSlug + "/save";
        UnityWebRequest request = CreateWebRequest(saveUrl, "GET");

        yield return request.SendWebRequest();

        Debug.Log(saveUrl);
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Could not find the save");
            Debug.Log(request.error);
            Debug.Log(Encoding.UTF8.GetString(request.downloadHandler.data));
        }
        else
        {
            Debug.Log("Save acquired");

            Debug.Log(request.downloadHandler.text);
            objToReceive.GetSaveData(TransformSaveData(JsonUtility.FromJson<data>(request.downloadHandler.text)));
        }
    }

    public static IEnumerator GetGamescore(string sceneSlug, IReceiveSave objToReceive)
    {
        string gamescoreUrl = "https://api.gamemind.com.br/" + GameManager.instance.accountType + "/game_scores/" + sceneSlug;
        Debug.Log(gamescoreUrl);

        UnityWebRequest request = CreateWebRequest(gamescoreUrl, "GET");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Could not find gamescore for: " + sceneSlug);
            Debug.Log(gamescoreUrl + " / " + request.error);
            Debug.Log(Encoding.UTF8.GetString(request.downloadHandler.data));
        }
        else
        {
            Debug.Log("Save acquired");
            Debug.Log(request.downloadHandler.text);

            data adata = JsonUtility.FromJson<data>(request.downloadHandler.text);
            objToReceive.GetSaveData(TransformSaveData(adata));
        }
    }

    private static data GenerateDataToSend(AdventureManager.AdventureSaveData savedata)
    {
        data dataToSend = new data()
        {
            record = new record
            {
                playtime = (int)savedata.elapsedTime,
                finished = savedata.finished,
                stars = savedata.stars,
                checkpoint = savedata.checkPointId,
                coin = savedata.curCoins,
                mistakes = savedata.mistakes
            }
        };

        return dataToSend;
    }

    private static AdventureManager.AdventureSaveData TransformSaveData(data dataReceived)
    {
        return new AdventureManager.AdventureSaveData
        {
            elapsedTime = dataReceived.record.playtime,
            finished = dataReceived.record.finished,
            stars = dataReceived.record.stars,
            checkPointId = dataReceived.record.checkpoint,
            curCoins = dataReceived.record.coin
        };
    }
}
