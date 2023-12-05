using UnityEngine;
using UnityEngine.Events;

public class ConfirmButtonBehaviour : InteractableObject
{
    [HideInInspector] public UnityEvent confirmed;

    public override void Interact(InteractionController interactor)
    {
        confirmed.Invoke();
    }
}