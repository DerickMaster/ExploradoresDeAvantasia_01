using UnityEngine;
using UnityEngine.Events;

public class LockBehaviour : InteractableObject
{
    public class FaceSwitchedEvent : UnityEvent<int> { };
    public FaceSwitchedEvent faceSwitchedEvent;

    Animator _myAnimator;

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
        faceSwitchedEvent = new FaceSwitchedEvent();
    }

    public override void Interact(InteractionController interactor)
    {
        _myAnimator.SetBool("Active", true);
        SwitchBlockedState();
    }

    public void StopTransition()
    {
        faceSwitchedEvent.Invoke(objectID);
        _myAnimator.SetBool("Active", false);
        SwitchBlockedState();
    }

    public void Deactive()
    {
        DeactivateInteractable();
    }

    public void ChangeDialMaterial(Material material)
    {
        Material[] materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
        materials[1] = material;
        GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;
    }
}