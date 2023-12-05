using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNPCInteraction : InteractableObject
{
    [HideInInspector] public bool _interacted = false;
    public DialogueObject _myDialog;
    public DialogueObject _repeatDialog;
    public GameObject _exclamation;
    public GameObject _questionMark;

    public override void Interact(InteractionController interactor)
    {
        GetComponent<Animator>().Play("Talking");
        if (_interacted && _repeatDialog != null)
        {
            DialogueBoxManager.Instance.PlayDialogue(_repeatDialog, interactor);
            return;
        }
        DialogueBoxManager.Instance.PlayDialogue(_myDialog, interactor);
        if (m_outline != null) SetActiveOutline(false);
        _exclamation.SetActive(false);
        _interacted = true;
    }

    public void ResetInteraction()
    {
        _interacted = false;
    }

    public void AlterDialogue(DialogueObject dialog, DialogueObject repeat)
    {
        _myDialog = dialog;
        _repeatDialog = repeat;
    }
}
