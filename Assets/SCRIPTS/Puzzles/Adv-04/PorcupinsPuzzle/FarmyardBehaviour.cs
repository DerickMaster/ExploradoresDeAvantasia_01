using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FarmyardBehaviour : InteractableObject
{
    [SerializeField] private int m_ExpectedId;
    [SerializeField] private int m_expectedCount;
    [SerializeField] GameObject[] m_slots;

    private int m_curCount = 0;
    private NumberSignBehaviour m_sign;
    
    [HideInInspector] public UnityEvent filledYard;

    private new void Start()
    {
        m_sign = GetComponentInChildren<NumberSignBehaviour>();
    }

    public override void Interact(InteractionController interactor)
    {
        if(interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == m_ExpectedId)
        {
            GameObject porcupim = GrabbableObjectInteractions.ReceiveItem(interactor, m_slots[m_curCount]);
            porcupim.GetComponent<PorcupimBehaviour>().SetIdle();
            m_curCount++;
            m_sign.TriggerChange(m_curCount);
            if(m_curCount == m_expectedCount)
            {
                DeactivateInteractable();
                filledYard.Invoke();
            }
        }
    }
}
