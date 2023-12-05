using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PieceSlotBehaviour : InteractableObject
{
    private InteractionController interactor;

    [SerializeField] GameObject[] slots;
    [SerializeField] GameObject[] transparentPieces;
    [SerializeField] int expectedId;
    [SerializeField] bool removable;

    private int curSlot;
    private int slotAmount;
    private bool filled;
    
    [HideInInspector] UnityEvent slotFilled;

    private new void Start()
    {
        slotAmount = slots.Length;
    }

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == expectedId && !filled)
        {
            GameObject received = GrabbableObjectInteractions.ReceiveItem(interactor, slots[curSlot]);
            received.transform.rotation = Quaternion.identity;
            transparentPieces[curSlot].SetActive(false);
            curSlot++;
            if (curSlot >= slotAmount)
            {
                Debug.Log("encheu");
                filled = true;
                curSlot = slots.Length;
                slotFilled.Invoke();

                if (!removable) DeactivateInteractable();
            }
        }
        else if(removable && !interactor.HoldingObject && curSlot > 0)
        {
            GrabbableObjectInteractions.GiveItem(interactor, slots[curSlot - 1].GetComponentInChildren<PieceBehaviour>().gameObject);
            curSlot--;
            filled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (filled) return;

        if (!interactor) interactor = other.GetComponent<InteractionController>();
        if (interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == expectedId)
        {
            transparentPieces[curSlot].SetActive(true);
            transparentPieces[curSlot].GetComponentInChildren<MeshFilter>().mesh = interactor.heldObject.GetComponentInChildren<MeshFilter>().mesh;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!filled) transparentPieces[curSlot].SetActive(false);
    }
}
