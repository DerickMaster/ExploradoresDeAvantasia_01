using System;
using UnityEngine;

public class AdventureManager : MonoBehaviour, IReceiveSave
{
    public static AdventureManager Instance;

    [SerializeField] public StageType stageType;
    [SerializeField] private StarsCalc starsCalc;
    [SerializeField] string sceneSlug;

    public bool localFinishedRun = false;
    CheckpointsManager checkPManager;
    CoinsManager coinsManager;
    TimerBehaviour timer;

    public enum StageType
    {
        NotSet,
        Adventure,
        Challenge,
        BossFight
    }

    private enum StarsCalc
    {
        Timer,
        Health
    }

    [Serializable]
    public struct AdventureSaveData
    {
        public int checkPointId;
        public int curCoins;
        public float elapsedTime;
        public bool finished;
        public int stars;
        public string[] mistakes;
    }

    [Serializable]
    private struct RunFeedbackData
    {
        public float sessionTime;
        public int mistakesAmount;
        public string mistakes;
    }

    private void Start()
    {
        Instance = this;

        checkPManager = FindObjectOfType<CheckpointsManager>();
        coinsManager = FindObjectOfType<CoinsManager>();
        timer = FindObjectOfType<TimerBehaviour>();

        if (stageType == StageType.Adventure)
        {
            try
            {
                StartCoroutine(APIInterface.GetSaveRequest(sceneSlug, this));
            }
            catch
            {
                Debug.Log("error in loading");
            }
        }
        else Debug.Log("Stage is not adventure, skipping load");
        
    }

    [ContextMenu("Test Save")]
    public void TestSave()
    {
        AdventureSaveData data = GenerateSaveData();
        Debug.Log(JsonUtility.ToJson(data));
        StartCoroutine(APIInterface.SendPostRequest(sceneSlug, data));
    }

    public void RestoreState(AdventureSaveData data)
    {
        Debug.Log(JsonUtility.ToJson(data));

        if (data.finished)
        {
            Debug.Log("last save finshed, ignoring load");
            return;
        }

        if(data.checkPointId <= 0 || data.elapsedTime < 1f)
        {
            Debug.Log("save checkpoint : " + data.checkPointId.ToString() + " elapsed time: " + data.elapsedTime.ToString() + ", ignoring load");
            return;
        }

        checkPManager.PutPlayerOnCheckPoint(data.checkPointId);
        coinsManager.SetCoinsAmount(data.curCoins);
        timer.ElapsedTime = data.elapsedTime;
    }

    public void SaveGame(int checkpointId)
    {
        CaptureState(checkpointId, GenerateSaveData());
    }

    public void ResetSave()
    {
        CaptureState(-1, new AdventureSaveData());
    }

    private void CaptureState(int checkpointId, AdventureSaveData saveData)
    {
        //Debug.Log(JsonUtility.ToJson(saveData));
        if(!saveData.finished && checkpointId <= 0)
        {
            StartCoroutine(APIInterface.SendPostRequest(sceneSlug, saveData));
        }
        else
        {
            StartCoroutine(APIInterface.SendPutRequest(sceneSlug, saveData));
        }
    }

    private AdventureSaveData GenerateSaveData()
    {
        int checkpointId = -1;
        try
        {
            checkpointId = checkPManager.lastSavedCheckpoint;
        }
        catch( NullReferenceException )
        {
            Debug.Log("checkpoint manager not found, ignoring checkpoint");
        }

        MistakesWatcher watcher = FindObjectOfType<MistakesWatcher>();
        int stars = 0;
        if (localFinishedRun) stars = GetStarsAmount();

        return new AdventureSaveData
        {
            stars = stars,
            checkPointId = checkpointId,
            curCoins = coinsManager.GetCoinsAmount(),
            elapsedTime = timer.ElapsedTime,
            finished = localFinishedRun,
            mistakes = watcher.GetMistakesArray()
        };
    }

    public int GetStarsAmount()
    {
        int amount = 1;
        switch (starsCalc)
        {
            case StarsCalc.Health:
                amount = (CharacterManager.Instance.GetCurrentCharacter().GetComponentInChildren<HealthManager>().curHealthAmount/2) - 1;
                break;
            case StarsCalc.Timer:
                amount = timer.GetStarsAmount();
                break;
        }
        if (amount <= 0) amount = 1;
        return amount;
    }

    public void GetSaveData(AdventureSaveData data)
    {
        RestoreState(data);
    }
}