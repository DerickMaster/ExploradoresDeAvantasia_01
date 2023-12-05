using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueBoxManager : MonoBehaviour
{
    public static DialogueBoxManager Instance { get; private set; }

    public StarterAssets.StarterAssetsInputs _input;
    public GameObject DialogueBox;
    public Text NameText;
    public Text DialogueText;

    public Image PortraitLeft;
    public Image PortraitRight;
    public Image _CButton;
    public Button soundBtn;

    public float textSpd = 50f;
    public bool auto = false;
    public float darkeningValue = 0.8f;

    public FMODUnity.EventReference currentDialogueTrack;
    public UnityEvent dialogueFinishedEvent;
    public InteractionController interactorRef;

    private void OnValidate()
    {
        if(NameText == null) GameObject.Find("NameText").GetComponent<Text>();
        if(DialogueText == null) GameObject.Find("DialogueText").GetComponent<Text>();
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlayDialogue(DialogueObject dialogueObj, InteractionController interactor,bool inCutscene = false, bool? auto = null)
    {
        if (auto == null) auto = this.auto;
        interactorRef = interactor;
        SetActiveDialogueBar(true);
        CanvasBehaviour.Instance.SetActiveBaseMobileUI(false);

        if (!Application.isMobilePlatform)
            _CButton.gameObject.SetActive(!inCutscene);
        
        soundBtn.gameObject.SetActive(!inCutscene);

        StartCoroutine(StepThroughDialogue(dialogueObj, interactor, (bool)auto));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObj, InteractionController interactor, bool auto)
    {
        interactor.EnterDialogueState();

        foreach (DialogueText dialogueText in dialogueObj.dialogue)
        {
            ShowPortraits(dialogueText.PortraitLeft, dialogueText.PortraitRight, dialogueText.DarkenedPortraitRight);
            PlayDialogueTrack(dialogueText.dialogueTrack);

            NameText.text = dialogueText.name;

            yield return WriteText(dialogueText.text);
            if (auto) yield return new WaitForSeconds(1f);
            else
            {
                interactor.DialogueFinishedPlaying();
                yield return new WaitUntil(() => interactor._input.advanceDialogue);
                interactor.DialogueStarted();
            }
        }
        EndCurrentDialogue();
    }

    public void EndCurrentDialogue()
    {
        Debug.Log("Ending current dialogue");

        _dialogueTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        StopAllCoroutines();
        SetActiveDialogueBar(false);
        CanvasBehaviour.Instance.SetActiveBaseMobileUI(true);

        try { interactorRef.ExitDialogueState(); } 
        catch { Debug.Log("Tried to end dialogue, interactor not found"); }

        interactorRef = null;
        dialogueFinishedEvent.Invoke();
    }

    private Coroutine WriteText(string text)
    {
        return StartCoroutine(TypeText(text, DialogueText));
    }

    private IEnumerator TypeText(string textToType, Text DialogueText)
    {
        float time = 0;
        int textIndex = 0;

        while (textIndex < textToType.Length)
        {
            time += Time.deltaTime * textSpd;
            textIndex = Mathf.FloorToInt(time);
            textIndex = Mathf.Clamp(textIndex, 0, textToType.Length);

            DialogueText.text = textToType.Substring(0, textIndex);

            yield return null;
        }

        DialogueText.text = textToType;
    }

    private void SetActiveDialogueBar(bool active)
    {
        DialogueBox.SetActive(active);
        PortraitLeft.gameObject.SetActive(active);
        PortraitRight.gameObject.SetActive(active);
    }

    private void ShowPortraits(Sprite spriteLeft, Sprite spriteRight, bool DarkenedPortraitRight)
    {
        PortraitLeft.sprite = spriteLeft;
        PortraitRight.sprite = spriteRight;
        if (DarkenedPortraitRight)
        {
            PortraitRight.color = new Color(darkeningValue, darkeningValue, darkeningValue);
            PortraitLeft.color = new Color(1, 1, 1);
        } 
        else
        {
            PortraitRight.color = new Color(1, 1, 1);
            PortraitLeft.color = new Color(darkeningValue, darkeningValue, darkeningValue);
        }
    }

    FMOD.Studio.EventInstance _dialogueTrack;
    private void PlayDialogueTrack(FMODUnity.EventReference dialogueTrack)
    {
        if (!dialogueTrack.IsNull)
        {
            currentDialogueTrack = dialogueTrack;
            _dialogueTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            try
            {
                _dialogueTrack = FMODUnity.RuntimeManager.CreateInstance(currentDialogueTrack);
                _dialogueTrack.start();
            }
            catch
            {
#if UNITY_EDITOR
                Debug.Log("no dialogue track found for " + currentDialogueTrack.Path);
#endif
            }
        }
    }

    public void PlayCurrentDialogueTrack()
    {
        FMODUnity.RuntimeManager.PlayOneShot(currentDialogueTrack, transform.position);
    }
}
