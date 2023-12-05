using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SyllabCompleteWordPuzzle : MonoBehaviour
{
    [SerializeField] private IncompleteWordManager _incompleteWordManager;
    [SerializeField] private LettersButtonManager _lettersManager;
    [SerializeField] private Text _completeWordUI;

    public string[] completeWordSyllabs;
    public string[] syllabsToShow;

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

        _incompleteWordManager.Initialize(completeWordSyllabs.Length);
        _incompleteWordManager.ChangeLetterSlotSize(110f, 120f);
        _lettersManager.Initialize(syllabsToShow);

        _completeWordUI.text = string.Join("",completeWordSyllabs);
    }

    public void SetPuzzle(string[] wordInSyllabs, string[] syllabs)
    {
        completeWordSyllabs = wordInSyllabs;
        syllabsToShow = syllabs;
    }

    private void ButtonClicked(string text, Button btnClicked)
    {
        bool correct = false;
        for (int i = 0; i < completeWordSyllabs.Length; i++)
        {
            if (completeWordSyllabs[i].Equals(text))
            {
                correct = true;
                if (_incompleteWordManager.InsertLetter(text, i))
                {
                    PuzzleCompleted();
                }
            }
        }

        if (!correct)
        {
            MistakeData data = new MistakeData { rightAnswer = string.Join("",completeWordSyllabs), mistakeMade = text };
            _wrongLetter.Invoke(_puzzleName, data);
        }

        _lettersManager.BlockButton(btnClicked, correct);
    }

    [HideInInspector] public UnityEvent _puzzleFinishedEvent;
    private void PuzzleCompleted()
    {
        GetComponent<ObjectSounds>().playObjectSound(0);
        _lettersManager.DeactiveAllButtons();
        Invoke(nameof(DeactivateObj), 2f);

        _puzzleFinishedEvent.Invoke();
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
