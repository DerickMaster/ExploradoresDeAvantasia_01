using UnityEngine;
using UnityEngine.Events;

public class SandwichPuzzleManager : MonoBehaviour, IMakeMistakes
{
    public SandwichBehaviour _sandwich;
    public IngredientBoxBehaviour[] _boxes;
    public int[] ExpectedIngredientOrder;
    public int currentIngredient = 0;
    public UnityEvent puzzleFinishedEvent;

    [SerializeField] private GameObject[] sandwicheStates;

    [SerializeField] string[] instrunctionPhrases;
    [SerializeField] string[] correctPhrases;
    [SerializeField] string[] incorrectPhrases;
    [SerializeField] float tempTextTime;

    public BlockedZoneBehaviour _myReminder;

    [HideInInspector] public UnityEvent<string,MistakeData> _wrongIngredient;
    public string _puzzleName;
    public MistakeData _mistakeDescription;

    private void Start()
    {
        _sandwich = GetComponentInChildren<SandwichBehaviour>();
        _boxes = GetComponentsInChildren<IngredientBoxBehaviour>();

        foreach (IngredientBoxBehaviour box in _boxes)
        {
            box.interacted.AddListener(CheckIngredientID);
        }
        _sandwich.ingredientAdded.AddListener(IngredientAdded);
        _sandwich.SwitchInteract(false);
    }

    public void StartPuzzle()
    {
        SendTempMessage(instrunctionPhrases[currentIngredient]);
        _myReminder.tempText = instrunctionPhrases[currentIngredient];
    }

    private void IngredientAdded(int id)
    {
        ChangeSandwichState(currentIngredient);
        _sandwich.SwitchInteract(false);

        currentIngredient++;
        if(currentIngredient >= ExpectedIngredientOrder.Length)
        {
            CompletePuzzle();
        }
        else
        {
            string[] texts = { correctPhrases[currentIngredient - 1], instrunctionPhrases[currentIngredient] };
            SendTempMessage(texts);
            _myReminder.tempText = instrunctionPhrases[currentIngredient];
        }
        foreach (IngredientBoxBehaviour boxBehaviour in _boxes)
        {
            if (boxBehaviour.blockInteract)
            {
                boxBehaviour.SwitchBlockedState();
                break;
            }
        }
    }
    public Sprite _portrait;
    private void SendTempMessage(string message)
    {
        CanvasBehaviour.Instance.SetActiveTempText(message, tempTextTime, _portrait);
    }
    
    private void SendTempMessage(string[] messages)
    {
        CanvasBehaviour.Instance.SetActiveTempText(messages, tempTextTime);
    }

    private void ChangeSandwichState(int newState)
    {
        if(newState > 0) sandwicheStates[newState - 1].SetActive(false);
        sandwicheStates[newState].SetActive(true);
    }

    public void CompletePuzzle()
    {
        foreach (IngredientBoxBehaviour box in _boxes)
        {
            box.BlockInteract();
        }
        this.puzzleFinishedEvent.Invoke();
        GetComponent<ObjectSounds>().playObjectSound(0);
    }

    public void CheckIngredientID(IngredientBoxBehaviour interactedBox)
    {
        if (interactedBox.IngredientID != ExpectedIngredientOrder[currentIngredient])
        {
            _wrongIngredient.Invoke(_puzzleName, GenerateMistakeData(ExpectedIngredientOrder[currentIngredient], interactedBox.IngredientID));
            SendTempMessage(incorrectPhrases[currentIngredient]);
            interactedBox.BlockBox();
        }
        else _sandwich.SwitchInteract(true);
    }

    private string GetIndredientName(int ingredientId)
    {
        switch (ingredientId)
        {
            case 4001:
                return "Pao";
            case 4002:
                return "Alface";
            case 4003:
                return "Queijo";
            case 4004:
                return "Tomate";
            default:
                return "Ingrediente invalido";
        }
    }

    private MistakeData GenerateMistakeData(int expectedId, int receivedId)
    {
        return new MistakeData
        {
            rightAnswer = GetIndredientName(expectedId),
            mistakeMade = GetIndredientName(receivedId)
        };
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return _wrongIngredient;
    }
}
