using UnityEngine;
using UnityEngine.Events;

public class FallPitBehaviour : MonoBehaviour
{
    [HideInInspector] public UnityEvent playerTriggeredZoneEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTriggeredZoneEvent.Invoke();
        }
    }
}
