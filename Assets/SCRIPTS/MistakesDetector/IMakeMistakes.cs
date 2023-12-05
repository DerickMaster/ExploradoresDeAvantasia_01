using UnityEngine.Events;

public interface IMakeMistakes
{
    public UnityEvent<string, MistakeData> GetMistakeEvent();
}