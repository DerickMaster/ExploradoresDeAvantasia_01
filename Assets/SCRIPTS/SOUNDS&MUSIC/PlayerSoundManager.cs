using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<string, FMODUnity.EventReference>, ISerializationCallbackReceiver 
    {
        [SerializeField]
        private List<string> soundNames = new List<string>();
        
        [SerializeField]
        private List<FMODUnity.EventReference> sounds = new List<FMODUnity.EventReference>();

        public void OnAfterDeserialize()
        {
            this.Clear();

            if (soundNames.Count != sounds.Count)
            {
                Debug.Log("test");
            }

            for (int i = 0; i < soundNames.Count; i++)
            {
                this.Add(soundNames[i], sounds[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            soundNames.Clear();
            sounds.Clear();
            foreach(KeyValuePair<string, FMODUnity.EventReference> pair in this)
            {
                soundNames.Add(pair.Key);
                sounds.Add(pair.Value);
            }
        }
    }

    [System.Serializable] public class SoundDictionary : SerializableDictionary<string, FMODUnity.EventReference> { };
    [SerializeField] private SoundDictionary sounds;
    [SerializeField] private string nameToAdd;
    [SerializeField] private FMODUnity.EventReference soundToAdd;

    private void playSound(string soundName)
    {
        FMODUnity.EventReference sound;
        if (sounds.TryGetValue(soundName, out sound))
        {
            FMODUnity.RuntimeManager.PlayOneShot(sound, transform.position);
        }
    }

    private FMODUnity.EventReference stepSound; 
    private int mode = 0;
    private void playStepSound(string soundName)
    {
        string soundToPlay = soundName + "_" + mode.ToString();
        
        if (sounds.TryGetValue(soundToPlay, out stepSound))
        {
            FMODUnity.RuntimeManager.PlayOneShot(stepSound, transform.position);
        }
    }

    public void ChangeStep(int newMode)
    {
        mode = newMode;
    }

    [ContextMenu("Add New Item")]
    private void AddNewEntry()
    {
        sounds.Add(nameToAdd, soundToAdd);
    }
}
