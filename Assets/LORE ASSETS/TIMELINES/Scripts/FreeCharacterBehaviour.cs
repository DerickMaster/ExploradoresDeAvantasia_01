using UnityEngine;
using StarterAssets;
using UnityEngine.Playables;

public class FreeCharacterBehaviour : PlayableBehaviour
{
    public bool locked = true;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (locked)
        {
            CharacterManager charManager = playerData as CharacterManager;
            charManager.GetCurrentCharacter().GetComponent<ThirdPersonController>().SwitchControl(false);
            locked = false;
        }
    }
}
