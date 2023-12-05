using UnityEngine;
using UnityEngine.Events;

public enum InteractableObjType
{
    Default,
    NPC,
    Laura,
    Tino,
    Bia,
    Caua,
    Other
}

public abstract class InteractableObject : MonoBehaviour
{
    public string InteractableObjName;
    //[SerializeField] protected int type = 0;
    public bool blockInteract;
    public int objectID;
    public bool singleInteraction = false;
    public InteractableObjType objType;

    [HideInInspector] public UnityEvent objInteractedEvent;
    public Outline m_outline { protected set; get; }
    [HideInInspector] public float m_outlineWidth;

    [ContextMenu("Switch blocked state")]
    public void SwitchBlockedState()
    {
        blockInteract = !blockInteract;
        InteractionButtonListManager.instance.UpdateButtonState(this);
    }

    protected void Start()
    {
        try
        {
            m_outline = GetComponentInChildren<Outline>();
            m_outlineWidth = m_outline.OutlineWidth;
            m_outline.OutlineMode = Outline.Mode.OutlineVisible;
        }
        catch
        {
            //Debug.Log(gameObject.name + "sem outline");
        }
    }

    public virtual void Interact(InteractionController interactor)
    {
        // override this
    }

    public virtual void Interact(InteractionController interactor, bool option)
    {
        // override this
    }

    protected void DeactivateInteractable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        InteractionButtonListManager.instance.RemoveButton(this);
    }

    public void SetAllowedInteraction(bool allow)
    {
        m_outline.enabled = allow;
        blockInteract = !allow;
    }

    protected void ReactivateInteractable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void SetActiveOutline(bool active)
    {
        //m_outline.enabled = active;
        if (active) m_outline.OutlineWidth = m_outlineWidth;
        else m_outline.OutlineWidth = 0f;

    }

    public InteractableObjType GetInteractionObjType()
    {
        return objType;
    }

    private void OnDisable()
    {
        try
        {
            InteractionButtonListManager.instance.RemoveButton(this);
        }
        catch
        {

        }
    }
}