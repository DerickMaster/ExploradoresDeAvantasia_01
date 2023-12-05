using UnityEngine;

public class ObjectHoldingGrabbable : InteractableObject
{
    public GameObject ObjectToGrab;
    public GameObject ObjectSlot;
    public override void Interact(InteractionController interactor)
    {
        if (!interactor.HoldingObject && ObjectToGrab != null)
        {
            ObjectToGrab = GrabbableObjectInteractions.GiveItem(interactor, ObjectToGrab);
        }
        else if(interactor.HoldingObject)
        {
            ObjectToGrab = interactor.PutDownObject();
            ObjectToGrab.transform.SetParent(this.gameObject.transform);

            ObjectToGrab.transform.localPosition = Vector3.zero;
        }
    }
}