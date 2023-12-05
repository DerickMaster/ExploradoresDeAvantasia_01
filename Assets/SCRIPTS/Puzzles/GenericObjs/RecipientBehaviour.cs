using UnityEngine;
using UnityEngine.Events;

public class RecipientBehaviour : InteractableObject
{
    [SerializeField] private int[] acceptedIds;
    [SerializeField] private int expectedAmount;
    [SerializeField] bool updateSign;
    [SerializeField] bool blockWhenFilled;

    public int curAmount { private set; get; }
    public NumberSignBehaviour m_Sign;
    public bool correct = false;

    [HideInInspector] public UnityEvent filledEvent;
    [HideInInspector] public UnityEvent<InteractionController> wrongObjAdded;
    [HideInInspector] public UnityEvent objAddedEvent;
    [HideInInspector] public UnityEvent expectedAmountSetEvent;

    private new void Start()
    {
        base.Start();
        curAmount = 0;
        m_Sign = GetComponentInChildren<NumberSignBehaviour>();
    }

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject)
        {
            if (CheckContains(interactor.heldObject.GetComponent<GrabbableObject>().objectID))
            {
                GrabbableObjectInteractions.SwallowItem(interactor);

                curAmount++;
                objAddedEvent.Invoke();

                if (updateSign) m_Sign.TriggerChange(curAmount);

                if (CheckFilled())
                {
                    filledEvent.Invoke();
                    if (blockWhenFilled) DeactivateInteractable();
                }
            }
            else
            {
                wrongObjAdded.Invoke(interactor);
            }
        }
    }

    public void ResetRecipient()
    {
        curAmount = 0;
        ReactivateInteractable();
    }

    public bool CheckContains(int objId)
    {
        foreach (int id in acceptedIds)
        {
            if (id == objId) return true;
        }
        return false;
    }

    public bool CheckFilled()
    {
        return curAmount == expectedAmount;
    }

    public void SetExpectedAmount(int amount = 0)
    {
        if(amount != 0) expectedAmount = amount;
        expectedAmountSetEvent.Invoke();
    }

    public void BlockInteract()
    {
        DeactivateInteractable();
    }

    public void SetHardMode(bool hardMode)
    {
        updateSign = !hardMode;
    }

    public int GetExpectedAmount()
    {
        return expectedAmount;
    }

    [ContextMenu("Test Filed")]
    public void TestFilled()
    {
        filledEvent.Invoke();
    }
}
