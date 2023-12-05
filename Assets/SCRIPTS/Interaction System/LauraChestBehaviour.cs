using UnityEngine.Events;
using UnityEngine;
public class LauraChestBehaviour : ChestObjectBehaviour
{
    public DialogueObject myDialog;
    public SimpleNPCInteraction _tiaDaLaura;
    public DialogueObject _tdlNewDialogue;

    [HideInInspector] public UnityEvent _puzzleFinished;

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == keyObjectID)
        {
            GrabbableObjectInteractions.SwallowItem(interactor);
            PlayAnimation();
            currentQtObject++;
            if (currentQtObject == qtObjectsExpected)
            {
                DialogueBoxManager.Instance.PlayDialogue(myDialog, interactor);
                _tiaDaLaura._repeatDialog = _tdlNewDialogue;
                reference.Interact(interactor);
                _puzzleFinished.Invoke();
            }
            SetActiveOutline(false);
            SwitchBlockedState();
        }
    }
}