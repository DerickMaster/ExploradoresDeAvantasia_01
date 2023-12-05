using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCNael : InteractableObject, ISaveable, IMakeMistakes
{
    enum NaelState
    {
        AskForCoinsState,
        WaitForCoinsState,
        FinishedState
    };

    public Animator _myAnim;
    public DialogueObject[] interactions;
    public GameObject coins;
    public int dialogueEntry = 0;
    public DoorBehaviour myDoor;
    private NaelState currentState;
    public GameObject _questExclamation;

    [HideInInspector] public UnityEvent<string, MistakeData> _wrongWay;
    public BlockedZoneBehaviour _wrongZone;

    private new void Start()
    {
        base.Start();
        _myAnim = GetComponent<Animator>();
        currentState = NaelState.AskForCoinsState;
        _questExclamation.SetActive(true);
        SetActiveOutline(true);
        _wrongZone.touched.AddListener(WrongWayTriggered);
        shopDoor.OpenDoor();
    }

    public override void Interact(InteractionController interactor)
    {
        objInteractedEvent.Invoke();
        switch (currentState)
        {
            case NaelState.AskForCoinsState:
                AskForCoinsBehaviour(interactor);
                break;
            case NaelState.WaitForCoinsState:
                WaitForCoinsBehaviour(interactor);
                break;
            case NaelState.FinishedState:
                FinishedBehaviour(interactor);
                break;
        }
    }

    public SimpleNPCInteraction _tiadaLaura;
    public DialogueObject _newDialogue, _newRepeat;
    public DirectionalArrowsBehaviour[] _act1Arrows;
    private void AskForCoinsBehaviour(InteractionController interactor)
    {
        _myAnim.SetTrigger("Welcoming");
        DialogueBoxManager.Instance.PlayDialogue(interactions[dialogueEntry], interactor);

        _tiadaLaura.AlterDialogue(_newDialogue, _newRepeat);
        _tiadaLaura.SetActiveOutline(true);

        coins.SetActive(true);
        foreach(DirectionalArrowsBehaviour arrow in _act1Arrows)
        {
            arrow.ArrowDirectionRight(false);
        }
        dialogueEntry++;
        currentState = NaelState.WaitForCoinsState;

        SetActiveOutline(false);
        coins.GetComponent<GrabbableObject>().obj_Grab_Drop.AddListener(ControlOutline);
    }
    
    private void ControlOutline(GrabbableObject obj)
    {
        SetActiveOutline(obj.grabbed);
    }

    [SerializeField] public DoorBehaviour shopDoor;
    private void WaitForCoinsBehaviour(InteractionController interactor)
    {
        _myAnim.SetTrigger("Talking");
        try
        {
            if (interactor.heldObject.GetComponent<GrabbableObject>().objectID == 135)
            {
                GrabbableObjectInteractions.SwallowItem(interactor);
                DialogueBoxManager.Instance.PlayDialogue(interactions[dialogueEntry], interactor);
                myDoor.OpenDoor();
                dialogueEntry++;
                currentState = NaelState.FinishedState;
                _questExclamation.SetActive(false);
                SetActiveOutline(false);
                _myAnim.SetTrigger("Giving");
                shopDoor.CloseDoor();
            }
        }
        catch
        {
            DialogueBoxManager.Instance.PlayDialogue(interactions[3], interactor);
        }
    }

    private void FinishedBehaviour(InteractionController interactor)
    {
        DialogueBoxManager.Instance.PlayDialogue(interactions[dialogueEntry], interactor);
        _myAnim.SetTrigger("Talking");
    }

    public object CaptureState()
    {
        return new SaveData
        {
            savedState = (int)currentState,
            currentDialogue = dialogueEntry
        };
    }

    public void RestoreState(object state)
    {
        SaveData saveData = (SaveData)state;

        this.currentState = (NaelState)saveData.savedState;
        this.dialogueEntry = saveData.currentDialogue;
    }

    void WrongWayTriggered(BlockedZoneBehaviour zone)
    {
        MistakeData mistake = new MistakeData
        {
            rightAnswer = "Pegou as moedas",
            mistakeMade = "Passou das moedas"
        };
        _wrongWay.Invoke("MoedasNael", mistake);
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return _wrongWay;
    }

    [System.Serializable]
    private struct SaveData
    {
        public int savedState;
        public int currentDialogue;
    }
}
