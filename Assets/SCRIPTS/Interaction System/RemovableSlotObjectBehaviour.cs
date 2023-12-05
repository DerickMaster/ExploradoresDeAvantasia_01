using UnityEngine;

public class RemovableSlotObjectBehaviour : SlottableObjectBehaviour
{
    public bool correctObjSet = false;

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject)
        {
            ReceiveObject(interactor);
            if (HeldObject.GetComponent<InteractableObject>().objectID == keyObjectID) correctObjSet = true;
        }
        else if(!interactor.HoldingObject && HeldObject != null)
        {
            RemoveObject(interactor);
            correctObjSet = false;
        }
    }
}