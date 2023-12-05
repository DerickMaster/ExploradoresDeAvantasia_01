using UnityEngine;
using UnityEngine.Playables;

public class AdvanceDialogueBehaviour : PlayableBehaviour
{
    public bool triggered = false;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!triggered)
        {
            CharacterManager charManager = playerData as CharacterManager;
            charManager.GetCurrentCharacter().GetComponentInChildren<InteractionController>()._input.AdvanceDialogueInput(true);
            triggered = true;
        }
    }
}
