using UnityEngine;
using UnityEngine.Playables;

public class FreeCharacterClip : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<FreeCharacterBehaviour>.Create(graph);

        //AdvanceDialogueBehaviour advanceDialogueBehaviour = playable.GetBehaviour();

        return playable;
    }
}
