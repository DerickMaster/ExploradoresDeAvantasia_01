using UnityEngine;
using UnityEngine.Events;

public class ActivationBookBehaviour : InteractableObject
{
    [HideInInspector] public UnityEvent<ActivationBookBehaviour, bool, bool> bookInteractedEvent;
    private bool open = false;
    private Animator m_Animator;

    private new void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
    }

    public override void Interact(InteractionController interactor)
    {
        open = !open;
        bookInteractedEvent.Invoke(this, open, interactor != null);
        m_Animator.SetBool("Active", open);
        DeactivateInteractable();
        Invoke(nameof(ReactivateInteractable), 2f);
    }
}