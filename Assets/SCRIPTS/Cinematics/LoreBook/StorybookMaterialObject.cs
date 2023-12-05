using UnityEngine;

[CreateAssetMenu(fileName = "NewStorybookMaterialNameArray", menuName = "Storybook Material Name Array")]
public class StorybookMaterialObject : ScriptableObject
{
    public string[] _materialNames;
    public FMODUnity.EventReference[] _audioTrack;
    public float[] _audioTime;
}
