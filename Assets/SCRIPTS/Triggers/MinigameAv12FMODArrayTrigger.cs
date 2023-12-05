using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class MinigameAv12FMODArrayTrigger : TriggerZone
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<string, EventReference>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<string> soundNames = new List<string>();

        [SerializeField]
        private List<EventReference> sounds = new List<EventReference>();

        public void OnAfterDeserialize()
        {
            Clear();

            if (soundNames.Count != sounds.Count)
            {
                Debug.Log("test");
            }

            for (int i = 0; i < soundNames.Count; i++)
            {
                Add(soundNames[i], sounds[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            soundNames.Clear();
            sounds.Clear();
            foreach (KeyValuePair<string, EventReference> pair in this)
            {
                soundNames.Add(pair.Key);
                sounds.Add(pair.Value);
            }
        }
    }

    [System.Serializable] public class SoundDictionary : SerializableDictionary<string, EventReference> { };
    [SerializeField] private SoundDictionary sounds;
    [SerializeField] private string nameToAdd;
    [SerializeField] private EventReference soundToAdd;

    EventReference _triggerTrack;
    FMOD.Studio.EventInstance _eventTrack;
    private void playSound(string soundName)
    {
        EventReference sound;
        if (sounds.TryGetValue(soundName, out sound))
        {
            _triggerTrack = sound;
            _eventTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            _eventTrack = RuntimeManager.CreateInstance(_triggerTrack);
            _eventTrack.start();

            //RuntimeManager.PlayOneShot(sound, transform.position);
        }
    }

    string _directionString;
    void GetSoundName(int index)
    {
        if(index == 0)
        {
            _directionString = "Atrás";
        }else if(index == 1)
        {
            _directionString = "Frente";
        }else if (index == 2)
        {
            _directionString = "Direita";
        }
        else
        {
            _directionString = "Esquerda";
        }
    }

    public ZoneManager _myManager;
    public override void Triggered(Collider other)
    {
        GetSoundName(_myManager._myRandom);
        playSound(_directionString);
        if (_hasText)
            ReminderText();
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }

    public bool _hasText;
    [SerializeField] string tempText;
    string _tempText;
    [SerializeField] float tempTextTime;
    protected virtual void ReminderText()
    {
        if (_directionString == "Atrás") _tempText = "Abra o portão ";
        else _tempText = tempText;
        CanvasBehaviour.Instance.SetActiveTempText(_tempText + _directionString, tempTextTime);
    }

    [ContextMenu("Add New Item")]
    private void AddNewEntry()
    {
        sounds.Add(nameToAdd, soundToAdd);
    }
}
