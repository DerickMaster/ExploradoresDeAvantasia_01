using UnityEngine;
using UnityEngine.Events;

public class GrabbableObject : InteractableObject
{
    [HideInInspector] public Collider m_collider;
    [HideInInspector] public Rigidbody m_rb;
    [HideInInspector] public UnityEvent<GrabbableObject> obj_Grab_Drop;
    [HideInInspector] public UnityEvent<GameObject> objDestroyedEvent;
    //[HideInInspector] public UnityEvent<GrabbableObject> obj_Grab_Drop;

    public bool _isHeavy = false;
    public bool grabbed { protected set; get; } = false;

    protected void Awake()
    {
        m_collider = gameObject.GetComponent<Collider>();
        m_rb = gameObject.GetComponent<Rigidbody>();
        //obj_Grab_Drop = new ObjectMovedEvent();
    }
    public override void Interact(InteractionController interactor)
    {
        if(interactor.HoldObject(this.gameObject, true))
        {
            SetPhysics(false);
        }
    }

    public void SetPhysics(bool active)
    {
        if(m_outline != null) SetActiveOutline(active);
        m_collider.enabled = active;
        if(m_rb) m_rb.isKinematic = !active;
        grabbed = !active;
        obj_Grab_Drop.Invoke(this);
    }

    public void GotSwallowed()
    {
        objDestroyedEvent.Invoke(gameObject);
    }

    public void ManualDestroy()
    {
        objDestroyedEvent.Invoke(gameObject);
        Destroy(this.gameObject);
    }
}
