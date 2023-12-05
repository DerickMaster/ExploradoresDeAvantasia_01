using UnityEngine;

public class PrintPuzzleActivator : InteractableObject
{
    private PrintPuzzleManager puzzleManager;
    private InteractionController interactor;
    public GameObject m_camera;
    private GameObject mainCamera;
    private new void Start()
    {
        base.Start();

        puzzleManager = GetComponentInParent<PrintPuzzleManager>();
        puzzleManager.closePuzzleEvent.AddListener(ClosePuzzle);
        mainCamera = Camera.main.gameObject;
        SetAllowedInteraction(false);
    }

    [ContextMenu("Interact Debug")]
    public void DebugInterat()
    {
        puzzleManager.Phrase = "TESTE TESTA TESTO";
        this.interactor = FindObjectOfType<InteractionController>();
        puzzleManager.enabled = true;
        interactor.SwitchActionMap("BoardControl");
        puzzleManager.Initialize(interactor);
        SwitchCamera(true);
    }
    
    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == 201)
        {
            puzzleManager.Phrase = interactor.heldObject.GetComponent<LetterObject>()._phrase;
            GrabbableObjectInteractions.SwallowItem(interactor);
            SetAllowedInteraction(false);
        }
        else
        {
            CanvasBehaviour.Instance.SetActiveTempText("Porfavor inserir uma carta", 3f);
            return;
        }
        this.interactor = interactor;
        puzzleManager.enabled = true;
        interactor.SwitchActionMap("BoardControl");
        puzzleManager.Initialize(interactor);
        SwitchCamera(true);
    }

    public void ActivateInteraction(GrabbableObject obj)
    {
        if(obj.grabbed) SetAllowedInteraction(true);
    }

    private void SwitchCamera(bool active)
    {
        mainCamera.SetActive(!active);
        m_camera.gameObject.SetActive(active);
    }

    [SerializeField] int _repeated = 0;
    private void ClosePuzzle()
    {
        _repeated++;
        SwitchCamera(false);
        CanvasBehaviour.Instance.SetActiveButtonList(true);
        CanvasBehaviour.Instance.SetActiveDPad(false);
        puzzleManager.enabled = false;
        interactor.SwitchActionMap("Player");
        puzzleManager.DeactivateSelector();
        if (_repeated == 3)DeactivateInteractable();
    }
}
