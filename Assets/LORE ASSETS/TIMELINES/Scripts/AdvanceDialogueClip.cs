using UnityEngine;
using UnityEngine.Playables;

public class AdvanceDialogueClip : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AdvanceDialogueBehaviour>.Create(graph);

        //AdvanceDialogueBehaviour advanceDialogueBehaviour = playable.GetBehaviour();

        return playable;
    }
}