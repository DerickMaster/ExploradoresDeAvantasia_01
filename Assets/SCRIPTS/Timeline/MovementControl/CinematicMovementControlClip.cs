using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CinematicMovementControlClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private CinematicMovementControlBehaviour template = new CinematicMovementControlBehaviour();

    public ClipCaps clipCaps { get { return ClipCaps.None; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<CinematicMovementControlBehaviour>.Create(graph, template);
    }
}
