using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCJustina : InteractableObject
{
    [SerializeField] DoorBehaviour door;
    [SerializeField] DialogueObject firstDialogue;
    [SerializeField] DialogueObject lastDialogue;
    [SerializeField] SandwichPuzzleManager sandwichPuzzle;
    private InteractionController interactor;

    public override void Interact(InteractionController interactor)
    {
        this.interactor = interactor;
        DialogueBoxManager.Instance.PlayDialogue(firstDialogue, interactor);
        DialogueBoxManager.Instance.dialogueFinishedEvent.AddListener(ActivatePuzzle);
    }

    private void ActivatePuzzle()
    {
        DialogueBoxManager.Instance.dialogueFinishedEvent.RemoveAllListeners();

        sandwichPuzzle.StartPuzzle();
        sandwichPuzzle.puzzleFinishedEvent.AddListener(PostPuzzle);
    }

    private void PostPuzzle()
    {
        DialogueBoxManager.Instance.PlayDialogue(lastDialogue, interactor);
        DialogueBoxManager.Instance.dialogueFinishedEvent.AddListener(OpenDoor);
    }

    private void OpenDoor()
    {
        DialogueBoxManager.Instance.dialogueFinishedEvent.RemoveAllListeners();

        door.Interact(interactor);
    }
}