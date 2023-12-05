using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class PorcupimBehaviour : GrabbableObject
{
    public enum PorcupimState
    {
        Eating,
        Moving,
        Grabbed
    }

    [System.Serializable]
    private struct FloatPair
    {
        public float min;
        public float max;
    }

    public class PorcupimEvent : UnityEvent<PorcupimBehaviour> { }

    private Animator m_animator;
    private PorcupimState m_state;
    private NavMeshAgent m_agent;
    public GameObject target;

    public PorcupimEvent FinishedEating;

    new private void Awake()
    {
        base.Awake();
        FinishedEating = new PorcupimEvent();
    }

    new void Start()
    {
        m_animator = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        m_state = PorcupimState.Eating;
        this.obj_Grab_Drop.AddListener(ObjectGrabbed);
    }

    private void ObjectGrabbed(GrabbableObject obj)
    {
        if (obj.grabbed)
        {
            Debug.Log("alo");
            m_state = PorcupimState.Grabbed;
            m_agent.isStopped = true;
            m_animator.SetBool("grabbed", true);
            m_animator.SetBool("moving", false);
            m_agent.enabled = false;
        }
        else
        {
            m_agent.enabled = true;
            Debug.Log("alo2");
            m_animator.SetBool("grabbed", false);
            GetNewTarget();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case PorcupimState.Eating:
                EatingBehaviour();
                break;
            case PorcupimState.Moving:
                MovingBehaviour();
                break;
            case PorcupimState.Grabbed:
                break;
        }
    }

    [SerializeField] FloatPair waitTimeRange;
    float waitTime;
    float elapseTime = 0;
    private void EatingBehaviour()
    {
        elapseTime += Time.deltaTime;
        if (elapseTime >= waitTime) GetNewTarget();
    }

    [SerializeField] FloatPair speedRange;
    private void GetNewTarget()
    {
        if (target) target.GetComponent<StopPoint>().occupied = false;
        FinishedEating.Invoke(this);

        m_agent.SetDestination(target.transform.position);
        m_agent.isStopped = false;
        m_state = PorcupimState.Moving;

        m_animator.SetBool("eating", false);
        m_animator.SetBool("moving", true);
        m_agent.speed = Random.Range(speedRange.min, speedRange.max);
    }

    [SerializeField] float stoppingDistance;
    private void MovingBehaviour()
    {
        if(m_agent.remainingDistance <= stoppingDistance)
        {
            m_agent.ResetPath();
            m_state = PorcupimState.Eating;
            elapseTime = 0;
            waitTime = Random.Range(waitTimeRange.min, waitTimeRange.max);

            m_animator.SetBool("moving", false);
            m_animator.SetBool("eating", true);
        }
    }

    public void SetIdle()
    {
        m_animator.SetBool("grabbed", false);
        m_state = PorcupimState.Grabbed;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
