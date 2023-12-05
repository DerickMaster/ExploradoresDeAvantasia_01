using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CinematicMovementControlBehaviour : PlayableBehaviour
{
    Transform _target;

    public bool _firstFrameTriggered = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!_firstFrameTriggered)
        {
            _target = playerData as Transform;
            GameObject.FindObjectOfType<MoveCharacterInPath>().SetPath(_target);
            _firstFrameTriggered = true;
        }
    }
}
