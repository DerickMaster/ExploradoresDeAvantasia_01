using StarterAssets;
using UnityEngine.Timeline;

[TrackBindingType(typeof(CharacterManager))]
[TrackClipType(typeof(LockCharacterClip))]
[TrackClipType(typeof(FreeCharacterClip))]
public class MovementControlTrack : TrackAsset
{ 
}
