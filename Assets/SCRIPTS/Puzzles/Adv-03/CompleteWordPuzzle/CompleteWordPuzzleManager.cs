using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CompleteWordPuzzleManager : MonoBehaviour, IMakeMistakes
{
    [SerializeField] private IncompleteWordManager _incompleteWordManager;
    [SerializeField] private LettersButtonManager _lettersManager;
    [SerializeField] private Text _completeWordUI;

    public string completeWord;
    public string lettersToShow;

    [HideInInspector] public UnityEvent wordFinished;
    [HideInInspector] public UnityEvent<string, MistakeData> _wrongLetter;
    public string _puzzleName;
    public MistakeData _mistakeDescription;


    private void Start()
    {
        _lettersManager.buttonClickedEvent.AddListener(ButtonClicked);
        InitializePuzzle();
    }

    [ContextMenu("Turn ON")]
    public void TurnOnPuzzle()
    {
        gameObject.SetActive(true);
        InitializePuzzle();
    }

    // Ordem: Bia / Caua / Laura / Tino
    [SerializeField] Image _curBackground;
    public Sprite[] _canvases;
    [SerializeField] int _index;
    private void InitializePuzzle()
    {
        _curBackground = GetComponent<Image>();
        _curBackground.sprite = _canvases[_index];
        _curBackground.color = Color.white;
        CanvasBehaviour.Instance.SwitchControls(true);
        CanvasBehaviour.Instance.SetActiveButtonList(false);

        InteractionController.Instance.GetComponentInParent<StarterAssets.ThirdPersonController>().SwitchControl(true);

        _incompleteWordManager.Initialize(completeWord.Length);
        _lettersManager.Initialize(lettersToShow);

        _completeWordUI.text = completeWord;
    }

    public void SetPuzzle(string word, string letters)
    {
        completeWord = word;
        lettersToShow = letters;
    }

    private void ButtonClicked(string letter, Button btnClicked)
    {
        bool correct = false;
        for (int i = 0; i < completeWord.Length; i++)
        {
            if (char.ToUpper(completeWord[i]).Equals(letter[0]))
            {
                correct = true;
                if (_incompleteWordManager.InsertLetter(letter, i))
                {
                    PuzzleCompleted();
                }
            }
        }

        if(!correct)
        {
            MistakeData data = new MistakeData { rightAnswer = completeWord, mistakeMade = letter };
            _wrongLetter.Invoke(_puzzleName, data);
        }

        _lettersManager.BlockButton(btnClicked, correct);
    }

    [HideInInspector] public UnityEvent _puzzleFinished;
    private void PuzzleCompleted()
    {
        GetComponent<ObjectSounds>().playObjectSound(0);
        _lettersManager.DeactiveAllButtons();
        Invoke(nameof(DeactivateObj), 2f);
        _puzzleFinished.Invoke();
    }

    private void DeactivateObj()
    {
        gameObject.SetActive(false);
        InteractionController.Instance.GetComponentInParent<StarterAssets.ThirdPersonController>().SwitchControl(false);
        CanvasBehaviour.Instance.SetActiveButtonList(true);
        wordFinished.Invoke();
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return _wrongLetter;
    }
}