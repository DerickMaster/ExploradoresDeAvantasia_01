using UnityEngine.Playables;

public class DialogueControlBehaviour : PlayableBehaviour
{
    public DialogueObject dialogue;
    public bool dialoguePlayed = false;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if(dialogue != null && !dialoguePlayed)
        {
            CharacterManager charManager = playerData as CharacterManager;
            DialogueBoxManager.Instance.PlayDialogue(dialogue, charManager.GetCurrentCharacter().GetComponentInChildren<InteractionController>(), true);
            dialoguePlayed = true;
        }
    }
}