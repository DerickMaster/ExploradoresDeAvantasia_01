using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    public DialogueObject dialogue;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueControlBehaviour>.Create(graph);

        DialogueControlBehaviour dialogueControlBehaviour = playable.GetBehaviour();
        dialogueControlBehaviour.dialogue = this.dialogue;

        return playable;
    }
}
