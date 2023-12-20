using UnityEngine.Events;
using UnityEngine;

public class Tutorial_01_ZoneTrigger : MonoBehaviour
{
    [HideInInspector]public UnityEvent _enteredZoneEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _enteredZoneEvent.Invoke();
    }
}
