using UnityEngine;

public class NPCTest : InteractableObject
{
    public DialogueObject m_Dialogue;

    public void SendDialogue(InteractionController interactor)
    {
        Debug.Log("Sending dialogue");
        DialogueBoxManager.Instance.PlayDialogue(m_Dialogue, interactor);
    }

    public override void Interact(InteractionController interactor)
    {
        SendDialogue(interactor);
    }
}
