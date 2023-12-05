using UnityEngine;
using UnityEngine.Events;

public class RockRecipientBehaviour : InteractableObject
{
    [SerializeField] private bool blockWhenFilled = true;
    [SerializeField] private int expectedId;
    [SerializeField] public int expectedCount = 0;
    [SerializeField] Material[] numMaterials;

    private int curCount = 0;
    private Animator m_animator;
    private CaixaBotEventListener m_listener;
    private TextMesh m_textMesh;

    [HideInInspector] public UnityEvent recipientFilledEvent;
    [HideInInspector] public bool filled;
    [SerializeField] public bool hardMode = false;
    [SerializeField] private bool minigame = false;

    private new void Start()
    {
        base.Start();

        m_animator = GetComponentInChildren<Animator>();
        if (!hardMode) 
        {
            m_textMesh = GetComponentInChildren<TextMesh>();
            UpdateNumberText();
        } 
        m_listener = GetComponentInChildren<CaixaBotEventListener>();
    }

    public int GetExpectedId()
    {
        return expectedId;
    }

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject)
        {
            int keyObjId = interactor.heldObject.GetComponent<InteractableObject>().objectID;

            if (keyObjId == expectedId)
            {
                m_animator.SetTrigger("Swallow");
                GrabbableObjectInteractions.SwallowItem(interactor);
                curCount++;
                if (!minigame) m_animator.SetInteger("Value", curCount);
                SetAllowedInteraction(false);
                if(!hardMode) UpdateNumberText();

                if (curCount == expectedCount)
                {
                    recipientFilledEvent.Invoke();
                    filled = true;
                    if (blockWhenFilled) 
                    {
                        PlaySuccessOrFailAnimation();
                        DeactivateInteractable();
                    }
                    
                }
                else if (curCount > expectedCount) 
                {
                    filled = false;
                } 
            }
            else
            {
                //expel item
            }
        }
    }

    public UnityEvent GetListenerEvent()
    {
        return m_listener.finishedMoving;
    }

    public void PlaySuccessOrFailAnimation()
    {
        if(minigame) m_animator.SetBool("Active", false);

        if(filled) m_animator.SetTrigger("Success");
        else m_animator.SetTrigger("Fail");
    }

    public void SetNumberMaterial()
    {
        if (!minigame) return; 
        m_animator.SetBool("Active", true);
        m_outline.enabled = false;

        Material[] materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
        materials[1] = numMaterials[expectedCount - 1];
        GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

        m_outline.enabled = true;
    }

    private void UpdateNumberText()
    {
        m_textMesh.text = curCount.ToString();
    }

    public void BlockInteract()
    {
        DeactivateInteractable();
    }

    public void SetHardMode(bool hardMode)
    {
        this.hardMode = hardMode;
        if(hardMode) m_textMesh.text = "";
    }
}