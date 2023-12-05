using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSounds : MonoBehaviour
{
    public Dictionary<int,FMOD.Studio.EventInstance> soundsOnLoop;
    public FMODUnity.EventReference[] soundEventList;
    public bool isRandom;

    private void Start()
    {
        soundsOnLoop = new Dictionary<int, FMOD.Studio.EventInstance>();
    }

    public void playObjectSound(int soundNumber)
    {
        if (isRandom)
        {
            FMODUnity.RuntimeManager.PlayOneShot(soundEventList[Random.Range(0, soundEventList.Length)], transform.position);
        } else
        {
            FMODUnity.RuntimeManager.PlayOneShot(soundEventList[soundNumber], transform.position);
        }
    }

    public void PlaySoundOnLoop(int soundNumber)
    {
        if (!soundsOnLoop.ContainsKey(soundNumber))
        {
            soundsOnLoop.Add(soundNumber, FMODUnity.RuntimeManager.CreateInstance(soundEventList[soundNumber]));
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundsOnLoop[soundNumber], transform);
            soundsOnLoop[soundNumber].start();
            soundsOnLoop[soundNumber].release();
        }
        else
        {
            Debug.Log("Sound is already being played");
        }
    }

    public void StopSoundOnLoop(int soundNumber)
    {
        if (soundsOnLoop.ContainsKey(soundNumber))
        {
            FMODUnity.RuntimeManager.DetachInstanceFromGameObject(soundsOnLoop[soundNumber]);
            soundsOnLoop[soundNumber].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            soundsOnLoop.Remove(soundNumber);
        }
        else
        {
            Debug.Log("Sound not found to remove");
        }
    }
}
