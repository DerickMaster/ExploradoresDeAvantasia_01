using UnityEngine;
using UnityEngine.Events;

public class PlatformBehaviour : MonoBehaviour
{
    enum PlatformState
    {
        Waiting,
        Moving
    }

    [SerializeField] GameObject platform;
    public Vector3 movementMod;
    public float speed;
    PlatformState state;
    StarterAssets.ThirdPersonController tpc;
    bool playerOnTop = false;

    [HideInInspector] public UnityEvent playerEnteredEvent;
    [HideInInspector] public UnityEvent playerExitedEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            tpc = other.GetComponent<StarterAssets.ThirdPersonController>();
            playerOnTop = true;
            playerEnteredEvent.Invoke();
        }
    }

    public void SetMoving(bool moving)
    {
        if (moving) state = PlatformState.Moving;
        else state = PlatformState.Waiting;
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerOnTop && tpc.Grounded)
        {
            if (state == PlatformState.Moving)
            {
                tpc.externalMod = movementMod;
                tpc.externalSpd = speed;
            }
            else
            {
                tpc.externalMod = Vector3.zero;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            playerExitedEvent.Invoke();
            tpc.externalMod = Vector3.zero;
            tpc = null;
            playerOnTop = false;
        }
    }
}
