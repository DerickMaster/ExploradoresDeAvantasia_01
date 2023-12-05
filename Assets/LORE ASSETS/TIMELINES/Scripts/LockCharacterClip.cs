using UnityEngine;
using UnityEngine.Playables;

public class LockCharacterClip : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LockCharacterBehaviour>.Create(graph);

        //AdvanceDialogueBehaviour advanceDialogueBehaviour = playable.GetBehaviour();

        return playable;
    }
}
