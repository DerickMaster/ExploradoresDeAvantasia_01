using UnityEngine;
using UnityEngine.Events;

public class GridSquareBehaviour : InteractableObject
{
    public class ObjectInsertedEvent : UnityEvent<bool, int> { }

    [SerializeField] private GameObject m_slot;
    [SerializeField] public int m_keyObjectID;
    public ObjectInsertedEvent objectInsertedEvent;

    private void Awake()
    {
        objectInsertedEvent = new ObjectInsertedEvent();
    }

    public override void Interact(InteractionController interactor)
    {
        if(interactor.HoldingObject)
        {
            int _objID = interactor.heldObject.GetComponent<InteractableObject>().objectID;
            GrabbableObjectInteractions.ReceiveItem(interactor, m_slot);
            objectInsertedEvent.Invoke(_objID == m_keyObjectID, _objID);
        }
    }

    public void SetActiveSquare(bool active)
    {
        if (active) 
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            blockInteract = true;
        } 
        else DeactivateInteractable();
    }
}
