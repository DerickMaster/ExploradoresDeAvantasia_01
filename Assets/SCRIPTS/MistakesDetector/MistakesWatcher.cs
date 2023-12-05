using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct MistakeData
{
    public string rightAnswer;
    public string mistakeMade;
}

public class MistakesWatcher : MonoBehaviour
{
    public string sceneName;
    private Dictionary<string, List<MistakeData>> mistakesList;

    [System.Serializable]
    private struct Mistake
    {
        public string puzzleName;
        public MistakeData[] mistakesCommited;
    }

    public string jsonString;
    void Awake()
    {
        mistakesList = new Dictionary<string, List<MistakeData>>();
        sceneName = SceneManager.GetActiveScene().name;

        foreach (MistakesMaker maker in FindObjectsOfType<MistakesMaker>())
        {
            maker.GetPuzzleReference().GetMistakeEvent().AddListener(MistakeMade);
        }
    }

    private void MistakeMade(string puzzleName, MistakeData mistake)
    {
        if (!mistakesList.ContainsKey(puzzleName))
        {
            mistakesList.Add(puzzleName, new List<MistakeData>(1));
        }
        mistakesList[puzzleName].Add(mistake);
    }

    [ContextMenu("Print Dictionary")]
    public void DebugPrintDict()
    {
        Debug.Log(PrintDictionary());
    }
    
    [System.Serializable]
    struct Test
    {
        public string[] arr;
    }

    [ContextMenu("Print Array")]
    public void DebugPrintArray()
    {
        Test test = new Test
        {
            arr = GetMistakesArray()
        };
        Debug.Log(JsonUtility.ToJson(test));
    }

    [System.Serializable]
    private struct Session
    {
        public Mistake[] mistakes;
    }

    public int GetMistakesAmount()
    {
        int totalSize = 0;
        foreach(KeyValuePair<string, List<MistakeData>> kvp in mistakesList)
        {
            totalSize += kvp.Value.Count;
        }
        return totalSize;
    }

    public string PrintDictionary()
    {
        Mistake[] tempMistakes = new Mistake[mistakesList.Count];
        jsonString = "";

        int count = 0;
        foreach (KeyValuePair<string, List<MistakeData>> kvp in mistakesList)
        {
            tempMistakes[count].puzzleName = kvp.Key;
            tempMistakes[count].mistakesCommited = kvp.Value.ToArray();
            count++;
        }

        Session session = new Session
        {
            mistakes = tempMistakes
        };

        return JsonUtility.ToJson(session);
    }

    public string[] GetMistakesArray()
    {
        Mistake[] tempMistakes = new Mistake[mistakesList.Count];
        jsonString = "";

        int amount = 0;
        int count = 0;
        foreach (KeyValuePair<string, List<MistakeData>> kvp in mistakesList)
        {
            tempMistakes[count].puzzleName = kvp.Key;
            tempMistakes[count].mistakesCommited = kvp.Value.ToArray();
            amount += kvp.Value.Count;
            count++;
        }
        count = 0;
        string[] mistakesStrs = new string[amount];
        for (int i = 0; i < tempMistakes.Length; i++)
        {
            for (int j = 0; j < tempMistakes[i].mistakesCommited.Length; j++)
            {
                mistakesStrs[count] = ConvertMistakeToString(tempMistakes[i].puzzleName, tempMistakes[i].mistakesCommited[j]);
                count++;
            }
        }
        return mistakesStrs;
    }

    public string ConvertMistakeToString(string puzzleName,MistakeData data)
    {
        string str = "";
        str += puzzleName + ",";
        str += data.mistakeMade + ",";
        str += data.rightAnswer;
        //Debug.Log(str);
        return str;
    }
}