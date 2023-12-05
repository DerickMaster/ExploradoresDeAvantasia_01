using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarBehaviour : ChestObjectBehaviour
{
    [SerializeField] GrabbableObject keyObj;
    private new void Start()
    {
        base.Start();
        blockInteract = true;
        if (!keyObj) keyObj = transform.parent.gameObject.GetComponentInChildren<GrabbableObject>();
        keyObj.obj_Grab_Drop.AddListener(UnlockBlock);
        SetActiveOutline(false);
    }

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == keyObjectID)
        {
            reference.Interact(interactor);
            GrabbableObjectInteractions.SwallowItem(interactor);
            PlayAnimation();
            SetActiveOutline(false);
            if (singleInteraction) DeactivateInteractable();
        }
    }

    private void UnlockBlock(GrabbableObject obj)
    {
        blockInteract = !obj.grabbed;
        SetActiveOutline(obj.grabbed);
    }
}
