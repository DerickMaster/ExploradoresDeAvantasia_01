using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Minigame07Manager : MonoBehaviour, IMakeMistakes
{
    [SerializeField] private int initialLettersAmount;
    [SerializeField] private int initialNumbersAmount;

    [SerializeField] private Mesh[] lettersMesh;
    private Dictionary<string, Mesh> meshDict;
    [SerializeField] private Mesh[] numbersMesh;

    [SerializeField] private OperationButtonBehaviour yesBtn;
    [SerializeField] private OperationButtonBehaviour noBtn;

    FreezingGroundBehaviour[] tiles;
    SpawningPortalBehaviour[] portals;
    LettersSpawnPool spawnPool;
    LetterScreenBehaviour screen;
    private Transform buttonsParent;

    [HideInInspector] public UnityEvent<string, MistakeData> mistakeEvent;
    readonly string letters = "ABCDEFGHIJKLMOPQRSTUVWXYZ";
    readonly string numbers = "123456789";

    private void Start()
    {
        tiles = FindObjectsOfType<FreezingGroundBehaviour>();

        buttonsParent = yesBtn.transform.parent;

        yesBtn.pressedEvent.AddListener(YesSelected);
        noBtn.pressedEvent.AddListener(NoSelected);

        buttonsParent.gameObject.SetActive(false);

        meshDict = new Dictionary<string, Mesh>(lettersMesh.Length);
        foreach(var item in lettersMesh)
        {
            meshDict.Add(item.name.Substring(item.name.Length - 1, 1), item);
        }
        screen = GetComponentInChildren<LetterScreenBehaviour>();
        portals = GetComponentsInChildren<SpawningPortalBehaviour>();
        spawnPool = GetComponentInChildren<LettersSpawnPool>();
        targetAmount = spawnPool.LetterPool.Count;

        RegisterLetters();

        foreach (var item in spawnPool.NumberPool)
        {
            item.refUnitKilled.AddListener(CharObjectDestroyed);
            int id = Random.Range(0, numbers.Length);
            item.UpdateChar(char.ToString(numbers.ElementAt(id)), numbersMesh[id]) ;
        }
        enabled = false;
    }

    [SerializeField] float freezeCooldown;
    [SerializeField] float freezeDelay;
    [SerializeField] int amountOfFreezes;
    float elapsedTime = 0f;
    private void Update()
    {
        if(elapsedTime > freezeCooldown)
        {
            for (int i = 0; i < amountOfFreezes; i++)
            {
                tiles[Random.Range(0, tiles.Length)].Freeze(freezeDelay);
            }
            elapsedTime = 0f;
        }
        elapsedTime += Time.deltaTime;
    }

    private void RegisterLetters()
    {
        foreach (var item in spawnPool.LetterPool)
        {
            item.refUnitKilled.AddListener(CharObjectDestroyed);
            string letter = char.ToString(letters.ElementAt(Random.Range(0, letters.Length)));
            item.UpdateChar(letter, meshDict[letter]);
        }
    }

    public void InitialSpawn()
    {
        for (int i = 0; i < initialLettersAmount; i++)
        {
            SpawnOnRandomPortal(true);
        }

        for (int i = 0; i < initialNumbersAmount; i++)
        {
            SpawnOnRandomPortal(false);
        }
    }

    private int amountDestroyed = 0;
    private int targetAmount;
    private void CharObjectDestroyed(CharObjectBehaviour objKilled)
    {
        if (objKilled.isLetter)
        {
            amountDestroyed++;
            if(amountDestroyed == targetAmount)
            {
                ShowQuestion();
            }
            screen.UpdateCurSlot(objKilled.myChar);
            SpawnOnRandomPortal(true);
        }
        else
        {
            mistakeEvent.Invoke("Numero destruido", new MistakeData { mistakeMade = objKilled.myChar, rightAnswer = "Letra" });
            SpawnOnRandomPortal(false);
        }

    }

    private void SpawnOnRandomPortal(bool isLetter)
    {
        SpawningPortalBehaviour portal = portals[Random.Range(0, portals.Length)];
        portal.SpawnLetters(1, isLetter);
    }

    [SerializeField, TextArea] string message;
    private System.Func<int, int, bool> comparision;
    int questionValue = 0;
    private void ShowQuestion()
    {
        int[] mods = { -1, 1 };
        int id = Random.Range(0, mods.Length);
        questionValue = amountDestroyed + mods[id];
        string strMod;
        if (id == 0)
        {
            comparision = CompareHigher;
            strMod = "maior";
        }
        else 
        {
            comparision = CompareLower;
            strMod = "menor";
        } 
        CanvasBehaviour.Instance.SetActiveTempText(message + strMod + " que " + questionValue.ToString() + " ?", 0f);
        buttonsParent.gameObject.SetActive(true);
    }

    private string rightAnswer;
    private void HigherPressed(int value)
    {
        rightAnswer = "menor";

        if (targetAmount > questionValue) GameEnd();
        else IncreasePool("maior");
    }
    
    private void LowerPressed(int value)
    {
        rightAnswer = "maior";

        if (targetAmount < questionValue) GameEnd();
        else IncreasePool("menor");
    }

    [SerializeField, TextArea] string _errorMessage;
    [SerializeField] Sprite _professorSprite;
    [SerializeField] FMODUnity.EventReference _errorDub;
    private void IncreasePool(string message)
    {
        mistakeEvent.Invoke("Errou a quantidade", new MistakeData { mistakeMade = message, rightAnswer = rightAnswer });

        CanvasBehaviour.Instance.DisableTempText();
        CanvasBehaviour.Instance.SetActiveTempText(_errorMessage, 2f, _professorSprite, _errorDub);
        int amountToAdd = 2;
        screen.RemoveCurSlot(amountToAdd);
        spawnPool.IncreaseLetterPool(amountToAdd);
        RegisterLetters();
        for (int i = 0; i < amountToAdd; i++)
        {
            SpawnOnRandomPortal(true);
        }
        amountDestroyed -= amountToAdd;
        RegisterLetters();
    }

    private void GameEnd()
    {
        CanvasBehaviour.Instance.DisableTempText();
        CanvasBehaviour.Instance.ActivateEndgameScreen();
        //CanvasBehaviour.Instance.SetActiveTempText("Vitoria", 2f);
    }

    private void YesSelected(int value)
    {
        if (CompareValues(targetAmount, questionValue, comparision))
        {
            GameEnd();
        }
        else IncreasePool("Teste");
    }

    private void NoSelected(int value)
    {
        if (!CompareValues(targetAmount, questionValue, comparision))
        {
            GameEnd();
        }
        else IncreasePool("Teste");
    }

    private bool CompareHigher(int a, int b)
    {
        return a > b;
    }

    private bool CompareLower(int a, int b)
    {
        return a < b;
    }

    private bool CompareValues(int a, int b, System.Func<int, int, bool> op)
    {
        return op(a, b);
    }

    UnityEvent<string, MistakeData> IMakeMistakes.GetMistakeEvent()
    {
        return mistakeEvent;
    }
}
