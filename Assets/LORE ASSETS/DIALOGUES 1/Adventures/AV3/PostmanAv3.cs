using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostmanAv3 : SimpleNPCInteraction
{
    public DialogueObject _responseText;
    public bool _puzzleFinished = false;
    [SerializeField] GameObject letters;
    [SerializeField] PrintPuzzleManager printPuzzle;
    [SerializeField] PrintPuzzleActivator printPuzzleActivator;
    [SerializeField] BlockedZoneBehaviour blockedZone;

    [SerializeField] Sprite _portrait;
    public override void Interact(InteractionController interactor)
    {
        if(_puzzleFinished)
        {
            CanvasBehaviour.Instance.SetActiveTempText("Obrigado pelas cartas", 2f, _portrait);
            blockedZone.allowPassage = true;
        }
        else if(!_interacted)
        {
            base.Interact(interactor);
            _interacted = true;
            letters.SetActive(true);
            printPuzzle.closePuzzleEvent.AddListener(OpenPassage);
            foreach(GrabbableObject grabbable in letters.GetComponentsInChildren<GrabbableObject>())
            {
                grabbable.obj_Grab_Drop.AddListener(printPuzzleActivator.ActivateInteraction);
            }
        }
        else
        {
            DialogueBoxManager.Instance.PlayDialogue(_responseText, interactor);
        }
    }

    private int completionTimes = 0;
    [SerializeField] private int expectedCompletionTimes;
    private void OpenPassage()
    {
        completionTimes++;
        if (completionTimes == expectedCompletionTimes) {
            _puzzleFinished = true;
            GetComponent<ObjectSounds>().playObjectSound(0);
        }
    }
}
