using System.Collections;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine;

public class PorcuMomBehaviour : MonoBehaviour
{
    public enum PorcumomState
    {
        Seeking,
        Preparing,
        Charging,
        Resting,
        Waiting
    }
    public PorcumomState m_state;
    
    [SerializeField] GameObject _player;
    Animator _animator;
    Collider m_collider;
    private NavMeshAgent m_agent;
    private Vector3 _target;

    [SerializeField] private Material damagedMat;
    protected ObjectMaterialController materialController;

    [HideInInspector] public UnityEvent finishedChargingEvent;
    [HideInInspector] public UnityEvent hitWallChargingEvent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.isStopped = true;
        m_collider = GetComponent<Collider>();

        materialController = GetComponentInChildren<ObjectMaterialController>();
    }

    private void Update()
    {
        switch (m_state)
        {
            case PorcumomState.Seeking:
                SeekingBehaviour();
                break;
            case PorcumomState.Preparing:
                PreparingBehaviour();
                break;
            case PorcumomState.Charging:
                ChargingBehaviour();
                break;
            case PorcumomState.Resting:
                RestingBehaviour();
                break;
            default:
                break;
        }
    }

    public void EnterSeekingMode()
    {
        m_state = PorcumomState.Seeking;
        elapseTime = 0f;
        _animator.SetBool("Seeking", true);
    }

    private float elapseTime = 0f;
    [SerializeField] float _seekingTime;
    private void SeekingBehaviour()
    {
        m_agent.Move(seekSpeed * Time.deltaTime * transform.forward);

        _target = CharacterManager.Instance.GetCurrentCharacter().transform.position;
        m_agent.SetDestination(_target);

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _target - transform.position , 2 * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection);

        elapseTime += Time.deltaTime;
        if (elapseTime >= _seekingTime) PrepareCharge();
    }

    
    [SerializeField] float _preparingTime;
    [SerializeField] float chargeTime;

    [ContextMenu("PrepareCharge")]
    private void PrepareCharge()
    {
        m_state = PorcumomState.Preparing;
        elapseTime = 0f;
        _animator.SetBool("Seeking", false);
        _animator.SetBool("Flying", true);
    }

    bool locked = false;
    private void PreparingBehaviour()
    {
        elapseTime += Time.deltaTime;
        if (!locked)
        {
            _target = CharacterManager.Instance.GetCurrentCharacter().transform.position;
            transform.rotation = Quaternion.LookRotation((_target - transform.position).normalized);
        }
        if (elapseTime >= _preparingTime) _animator.SetBool("Flying", false);
    }

    
    [System.Serializable]
    private struct HitboxInfo
    {
        public Vector3 hitboxSize;
        public float forwardOffset;
        public float upOffset;
        public LayerMask mask;
    }

    [SerializeField] HitboxInfo hitboxInfo;
    [SerializeField] GameObject hitboxOrigin;

    private float lastDist;
    private void ChargingBehaviour()
    {
        m_agent.Move(chargeSpeed * Time.deltaTime * moveDirection);

        hitboxPos = (transform.position + transform.forward * hitboxInfo.forwardOffset + transform.up * hitboxInfo.upOffset);
        Collider[] cols = Physics.OverlapBox(hitboxPos, hitboxInfo.hitboxSize, transform.rotation, hitboxInfo.mask);
        if(cols.Length > 0)
        {
            foreach (var item in cols)
            {
                Debug.Log(item.name);
                if(item.transform != transform)
                    HitWall();
            }
        }

        //Debug.Log(lastDist);
        //Debug.Log(m_agent.remainingDistance);

        if (m_agent.remainingDistance < 1f || Vector3.Distance(transform.position, m_agent.destination) - lastDist > 1f)
        {
            StopCharge();
            finishedChargingEvent.Invoke();
            
        }
        lastDist = m_agent.remainingDistance;
    }

    private void HitWall()
    {
        StopCharge();
        _animator.SetTrigger("Bonk");
        hitWallChargingEvent.Invoke();
    }

    private void StopCharge()
    {
        StartRest();
        _animator.SetBool("Charging", false);
        locked = false;
    }

    [SerializeField] private float seekSpeed;
    [SerializeField] private float seekAcceleration;
    private void StartRest()
    {
        m_state = PorcumomState.Resting;
        m_agent.speed = seekSpeed;
        m_agent.acceleration = seekAcceleration;
        m_agent.autoBraking = true;
        elapseTime = 0f;
        m_collider.isTrigger = false;
    }

    private void RestingBehaviour() {}

    //called in event in animation
    private void StartCountdownToCharge()
    {
        SetChargeDirection();
        StartCoroutine(ChargeCountdown());
    }

    IEnumerator ChargeCountdown()
    {
        yield return new WaitForSeconds(chargeTime);

        StartCharging();
    }

    [SerializeField] private float maxChargeDist;
    [SerializeField] private float chargeSpeed;
    [SerializeField] LayerMask mask;
    [SerializeField] Vector3 offset;
    private Vector3 moveDirection;

    private void SetChargeDirection()
    {
        elapseTime = 0f;
        moveDirection = (_target - transform.position).normalized;
        moveDirection.y = 0;

        Debug.DrawRay(transform.position, moveDirection * maxChargeDist, Color.red, 10f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + offset, moveDirection, out hit, maxChargeDist, mask))
        {
            Debug.Log(hit.collider.name + " / " + hit.point);
            m_agent.SetDestination(hit.point);
        }
        else
        {
            Debug.Log("nada encontrado");
            m_agent.SetDestination(_target + (moveDirection * maxChargeDist));
            Debug.DrawRay(_target + (moveDirection * maxChargeDist), Vector3.up * 4f, Color.blue, 10f);
        }
        locked = true;
    }

    private void StartCharging()
    {
        _animator.SetBool("Charging", true);

        lastDist = Vector3.Distance(transform.position, m_agent.destination);
        m_state = PorcumomState.Charging;
        m_collider.isTrigger = true;
    }

    [ContextMenu("Start")]
    public void StartFight()
    {
        //inicializar tudo que precisa
        _player = CharacterManager.Instance.GetCurrentCharacter();
        _target = _player.transform.position;
        EnterSeekingMode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<HurtBox>().TakeDamage();
        }
    }

    public void TakeDamage()
    {
        StartCoroutine(TakeDamageCoroutine());
    }

    private IEnumerator TakeDamageCoroutine()
    {
        materialController.AddMaterial(damagedMat);
        yield return new WaitForSeconds(2f);
        materialController.RemoveMaterial(damagedMat.name);
    }

    private Vector3 hitboxPos;
    private void OnDrawGizmosSelected()
    {
        hitboxPos = (transform.position + (transform.forward * hitboxInfo.forwardOffset) + (transform.up * hitboxInfo.upOffset));
        Gizmos.DrawCube(hitboxPos, hitboxInfo.hitboxSize * 2);
    }

    /*
    private void DebugHitbox()
    {
        Collider[] cols = Physics.OverlapBox(hitboxPos, hitboxInfo.hitboxSize, transform.rotation, hitboxInfo.mask);
        if (cols.Length > 0)
        {
            foreach (var item in cols)
            {
                Debug.Log(item.name);
            }
        }
    }\
    */
}
