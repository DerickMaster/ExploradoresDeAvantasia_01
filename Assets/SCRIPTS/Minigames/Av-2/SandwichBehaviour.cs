using UnityEngine;
using UnityEngine.Events;

public class SandwichBehaviour : InteractableObject
{
    public class IngredientAddedEvent : UnityEvent<int> { }

    public bool completed = false;
    public IngredientAddedEvent ingredientAdded;

    private void Awake()
    {
        ingredientAdded = new IngredientAddedEvent();
    }

    public override void Interact(InteractionController interactor)
    {
        if (!interactor.HoldingObject) return;

        int id = interactor.heldObject.GetComponent<InteractableObject>().objectID;

        if ( 4001 <= id && id <= 4004)
        {
            ingredientAdded.Invoke(id);
        }

        GrabbableObjectInteractions.SwallowItem(interactor);
    }

    public void SwitchInteract(bool allowed)
    {
        if (!m_outline) m_outline = GetComponentInChildren<Outline>(); 
        m_outline.enabled = allowed;
        blockInteract = !allowed;
    }
}