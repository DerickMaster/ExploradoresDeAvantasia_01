using UnityEngine;
using UnityEngine.Events;

public class FreezingGroundEventListener : MonoBehaviour
{
    [HideInInspector] public UnityEvent disableEvent;
    public void DisableMesh()
    {
        disableEvent.Invoke();
    }
}
