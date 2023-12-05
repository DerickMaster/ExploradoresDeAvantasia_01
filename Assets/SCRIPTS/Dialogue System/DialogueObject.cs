using UnityEngine;

[System.Serializable]
public struct DialogueText
{
    public Sprite PortraitLeft;
    public Sprite PortraitRight;
    [Space(15)]
    public bool DarkenedPortraitRight;
    [Space(15)]
    public string name;
    [TextArea] public string text;
    public FMODUnity.EventReference dialogueTrack;
    
}
[CreateAssetMenu(menuName = "DialogueSystem/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    public DialogueText[] dialogue;
}