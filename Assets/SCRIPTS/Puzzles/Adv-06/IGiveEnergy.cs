using UnityEngine.Events;

public interface IGiveEnergy
{
    public UnityEvent<int> GetEvent();
}