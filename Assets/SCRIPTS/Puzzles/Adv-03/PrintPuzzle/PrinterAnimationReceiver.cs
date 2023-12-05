using UnityEngine;
using UnityEngine.Events;

public class PrinterAnimationReceiver : MonoBehaviour
{
    public UnityEvent stampEvent;

    public void Stamp()
    {
        stampEvent.Invoke();
    }
}
