using UnityEngine;
using UnityEngine.Events;

public class FurnaceSocketBehaviour : InteractableObject
{
    [SerializeField] private int m_keyObjId;
    public UnityEvent objInsertedEvent;

    public override void Interact(InteractionController interactor)
    {
        if(interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == m_keyObjId)
        {
            GrabbableObjectInteractions.SwallowItem(interactor);
            objInsertedEvent.Invoke();
            SetActive();
        }
    }

    public void SetActive()
    {
        GetComponent<Animator>().SetBool("Active", true);
    }
}
