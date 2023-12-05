using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Collections;

public class ThrashCollectorBehaviour : InteractableObject
{
    public class ReachedTargetEvent : UnityEvent<ThrashCollectorBehaviour> { }

    public int m_KeyObjectID;
    public int _receivedItemID;
    private NavMeshAgent m_agent;
    private Vector3 MainPosition;
    private Vector3 InitialPosition;

    public float proximityDistance;
    public ReachedTargetEvent reachedMain;

    private Animator m_animator;

    private void Awake()
    {
        reachedMain = new ReachedTargetEvent();
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    [SerializeField] Vector3 offset;
    public void Initialize(Vector3 pos)
    {
        MainPosition = pos + offset;
        InitialPosition = transform.position;
    }

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject)
        {
            _receivedItemID = interactor.heldObject.GetComponent<InteractableObject>().objectID;
            GrabbableObjectInteractions.SwallowItem(interactor);
            m_animator.SetBool("Collect", true);
            DeactivateInteractable();
        }
    }

    private float maxHeight;
    public float finalHeight;

    public void MoveToMain()
    {
        m_animator.SetBool("Collect", false);
        m_animator.SetBool("Moving", true);

        if (m_agent.SetDestination(MainPosition)) StartCoroutine(CheckProximity(MainPosition, false, 0f, finalHeight));
        else Debug.Log("Couldnt find path"); 
    }

    public void ReturnToInitialPosition()
    {
        m_animator.SetBool("Drop", false);
        m_agent.stoppingDistance = 0f;
        if (m_agent.SetDestination(InitialPosition)) StartCoroutine(CheckProximity(InitialPosition, true, maxHeight, 0f));
        else Debug.Log("Couldnt find path");
    }

    private IEnumerator CheckProximity(Vector3 targetPos, bool returning, float initialHeight, float finalHeight)
    {
        m_agent.baseOffset = initialHeight;

        Debug.Log(m_agent.remainingDistance);
        float totalDis = Vector3.Distance(transform.position, targetPos);
        Debug.Log(totalDis);
        float curDis = 1f;
        while(curDis > 0f)
        {
            
            curDis = Vector3.Distance(transform.position, targetPos);
            m_agent.baseOffset = Mathf.Lerp(finalHeight, initialHeight, curDis / totalDis);
            Debug.Log(curDis);
            yield return null;
            curDis = m_agent.remainingDistance;
        }

        maxHeight = m_agent.baseOffset;

        if (returning)
        {
            m_animator.SetBool("Moving", false);
            ReactivateInteractable();
        }
        else
        {
            m_animator.SetBool("Drop", true);
        }
    }

    public void DropedThrash()
    {
        reachedMain.Invoke(this);
    }

    public bool CheckReceivedItem()
    {
        return _receivedItemID == m_KeyObjectID;
    }
}