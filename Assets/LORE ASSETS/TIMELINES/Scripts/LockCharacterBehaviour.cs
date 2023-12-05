using UnityEngine;
using StarterAssets;
using UnityEngine.Playables;

public class LockCharacterBehaviour : PlayableBehaviour
{
    public bool locked = false;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!locked)
        {
            CharacterManager charManager = playerData as CharacterManager;
            if (charManager == null) charManager = CharacterManager.Instance;

            charManager.GetCurrentCharacter().GetComponent<ThirdPersonController>().SwitchControl(true);
            locked = true;
        }
    }
}
