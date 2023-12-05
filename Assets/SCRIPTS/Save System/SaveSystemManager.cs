using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance;
    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }
    private string SavePath => $"{Application.persistentDataPath}/save.txt";

    [ContextMenu("ShowDataPath")]
    private void ShowDataPath()
    {
        Debug.Log(SavePath);
    }

    [ContextMenu("Save")]
    public void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        var state = LoadFile();
        RestoreState(state);
    }

    [ContextMenu("DeleteFile")]
    public void ResetSave()
    {
        if (File.Exists(SavePath)) File.Delete(SavePath);
    }

    private void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(SavePath)) return new Dictionary<string, object>();

        using(FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Position = 0;
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }
    
    private void RestoreState(Dictionary<string, object> state)
    {
        foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            if(state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }

}
