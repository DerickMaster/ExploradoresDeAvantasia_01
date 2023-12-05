using UnityEngine.Events;

public interface ITrap
{
    public UnityEvent GetTrapEvent();
    public UnityEvent GetTrapFinishedEvent();
}
