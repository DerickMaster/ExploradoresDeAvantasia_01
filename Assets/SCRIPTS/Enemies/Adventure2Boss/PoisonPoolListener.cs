using UnityEngine;
using UnityEngine.Events;

public class PoisonPoolListener : MonoBehaviour
{
    [HideInInspector] public UnityEvent<bool> appearEvent;

    public void Appear()
    {
        appearEvent.Invoke(true);
    }
    
    public void Disappear()
    {
        appearEvent.Invoke(false);
    }
}
