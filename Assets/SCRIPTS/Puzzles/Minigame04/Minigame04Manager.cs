using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Minigame04Manager : MonoBehaviour, IEndGame, IMakeMistakes
{
    [System.Serializable]
    private struct PuzzleCombination
    {
        public string name;
        public string letters;
    }

    RecipientBehaviour[] recipients;
    int curRecipient = 0;
    int failurelCount = 0;
    bool hardMode = false;
    RockSpawner[] spawners;

    [SerializeField] string puzzleName;
    [SerializeField] int failureAllowed;
    [SerializeField] NumberSignBehaviour numberSign;
    [SerializeField] CompleteWordPuzzleManager completeWordManager;

    [SerializeField] string[] names_Combinations;
    Dictionary<int, List<PuzzleCombination>> combinations; 
    [HideInInspector] public UnityEvent<bool, float> gameEndEvent;
    [HideInInspector] public UnityEvent<string, MistakeData> mistakeMadeEvent;


    private void Start()
    {
        spawners = GetComponentsInChildren<RockSpawner>();
        completeWordManager = transform.parent.GetComponentInChildren<CompleteWordPuzzleManager>();
        completeWordManager.gameObject.SetActive(false);
        completeWordManager._wrongLetter.AddListener(WrongLetterClicked);

        //UIDifficultyDetector difficultyDetector = FindObjectOfType<UIDifficultyDetector>();
        //difficultyDetector.easyChosenEvent.AddListener(SetAsEasy);
        //difficultyDetector.hardChosenEvent.AddListener(SetAsHard);

        GenerateDict();

        InitializePuzzle();
    }

    [ContextMenu("TestGenerate")]
    private void GenerateDict()
    {
        combinations = new Dictionary<int, List<PuzzleCombination>>();
        foreach(string name_Comb in names_Combinations)
        {
            string[] temp = name_Comb.Split('/');
            int wordLength = temp[0].Length;
            if (!combinations.ContainsKey(wordLength)) combinations.Add(wordLength, new List<PuzzleCombination>());
            combinations[wordLength].Add(new PuzzleCombination { name = temp[0], letters = temp[1] });
        }
    }

    [SerializeField] PlayableDirector cutscene;
    private void InitializePuzzle()
    {
        numberSign = GetComponentInChildren<NumberSignBehaviour>();
        if (hardMode) numberSign.gameObject.SetActive(false);
        else numberSign.TriggerChange(0);

        recipients = GetComponentsInChildren<RecipientBehaviour>();
        foreach (RecipientBehaviour recipient in recipients)
        {
            recipient.gameObject.SetActive(false);
            recipient.GetComponentInChildren<CaixaBotEventListener>().finishedMoving.AddListener(PrepareNextRecipient);
            recipient.SetHardMode(hardMode);
            recipient.m_Sign = numberSign;
            recipient.objAddedEvent.AddListener(RespawnRocks);
        }

        GetComponentInChildren<ConfirmButtonBehaviour>().confirmed.AddListener(CheckRecipient);
        completeWordManager.wordFinished.AddListener(AdvanceState);

        recipients[curRecipient].gameObject.SetActive(true);
        recipients[curRecipient].GetComponentInChildren<CaixaBotEventListener>().SetAnimationActive(true);

        GenerateAmount();
        foreach (var spawner in spawners)
        {
            spawner.SpawnRocks(6);
        }

        try{ cutscene.Play(); }
        catch { Debug.Log("Cutscene nao determinada"); }
        
    }

    private void RespawnRocks()
    {
        foreach (var spawner in spawners)
        {
            spawner.CheckRespawnRock();
        }
    }

    [SerializeField] int minAmount;
    [SerializeField] int maxAmount;
    private void GenerateAmount()
    {
        int rng = Random.Range(minAmount, maxAmount + 1);

        recipients[curRecipient].SetExpectedAmount(rng);
        recipients[curRecipient].GetComponentInChildren<CaixaBotEventListener>().SetMaterial(rng);

        int randomCombination = Random.Range(0, combinations[rng].Count);

        completeWordManager.SetPuzzle(combinations[rng][randomCombination].name, combinations[rng][randomCombination].letters);
    }

    [SerializeField] Sprite _errorSprite;
    [SerializeField] FMODUnity.EventReference _errorDub;
    private void CheckRecipient()
    {
        recipients[curRecipient].BlockInteract();

        if (!recipients[curRecipient].CheckFilled())
        {
            recipients[curRecipient].GetComponentInChildren<CaixaBotEventListener>().PlaySuccessOrFailAnimation(false);// play fail animation
            CanvasBehaviour.Instance.SetActiveTempText("Errou!É melhor contar novamente.", 3f, _errorSprite, _errorDub);
            failurelCount++;
            MistakeData data = new MistakeData { rightAnswer = recipients[curRecipient].GetExpectedAmount().ToString(), mistakeMade = recipients[curRecipient].curAmount.ToString() };
            mistakeMadeEvent.Invoke(puzzleName, data);
            if (failurelCount > failureAllowed)
            {
                GameOver();
                return;
            }
        }
        else 
        {
            recipients[curRecipient].correct = true;
            CanvasBehaviour.Instance.SetActiveTempText("Agora escreva o nome indicado", 3f);
            completeWordManager.TurnOnPuzzle();
        }
        
    }

    private void AdvanceState()
    {
        recipients[curRecipient].GetComponentInChildren<CaixaBotEventListener>().PlaySuccessOrFailAnimation(true); //play succeess animation
    }

    private void PrepareNextRecipient()
    {
        recipients[curRecipient].gameObject.SetActive(false);

        if (recipients[curRecipient].correct) 
            curRecipient++;
        else
        {
            recipients[curRecipient].ResetRecipient();
        }

        if (curRecipient >= recipients.Length)
        {
            Finish();
        }
        else
        {
            recipients[curRecipient].gameObject.SetActive(true);
            recipients[curRecipient].GetComponentInChildren<CaixaBotEventListener>().SetAnimationActive(true);
            numberSign.TriggerChange(0);
            GenerateAmount();
        }
    }

    private void WrongLetterClicked(string puzzleName, MistakeData data)
    {
        CanvasBehaviour.Instance.SetActiveTempText("Errou! Essa letra não vai aí.", 3f);
        mistakeMadeEvent.Invoke(puzzleName, data);
    }

    private void Finish()
    {
        CanvasBehaviour.Instance.ActivateEndgameScreen();
        //gameEndEvent.Invoke();
    }

    private void GameOver()
    {
        CanvasBehaviour.Instance.ActivateEndgameScreen(false);
        //gameEndEvent.Invoke();
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return mistakeMadeEvent;
    }

    public UnityEvent<bool, float> GetEndGameEvent()
    {
        return gameEndEvent;
    }
}
