using UnityEngine;
using UnityEngine.Events;

public class FlowersGateBehaviour : DoorBehaviour
{
    private int ActivatedCount = 0;
    [SerializeField] private int ExpectedCount;
    public UnityEvent flowerInputed;

    private void Awake()
    {
        flowerInputed = new UnityEvent();
    }
    public override void Interact(InteractionController interactor)
    {
        ActivatedCount++;
        if(ActivatedCount >= ExpectedCount)
        {
            OpenDoor();
        }
        flowerInputed.Invoke();
    }
}
