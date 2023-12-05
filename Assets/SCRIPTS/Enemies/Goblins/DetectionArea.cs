using UnityEngine;
using UnityEngine.Events;

public class DetectionArea : MonoBehaviour
{
    public GameObject player;

    public UnityEvent PlayerDetected;
    public UnityEvent PlayerLost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            player = other.gameObject;
            PlayerDetected.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerLost.Invoke();
        }
    }
}
