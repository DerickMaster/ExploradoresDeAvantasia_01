using UnityEngine;
using UnityEngine.Events;

public class BookcaseGearReceiver : InteractableObject
{
    [HideInInspector] public UnityEvent gearInteracted;

    public override void Interact(InteractionController interactor)
    {
        gearInteracted.Invoke();
    }
}