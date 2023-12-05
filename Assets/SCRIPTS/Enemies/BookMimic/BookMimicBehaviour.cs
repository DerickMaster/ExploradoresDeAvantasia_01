using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BookMimicBehaviour : EnemyBehaviour
{
    private enum MimicState
    {
        Inactive,
        Active,
        Attacking
    }

    MimicState m_state;
    Collider m_detectionZone;
    NavMeshAgent agent;
    GameObject playerRef;
    CharacterManager charManager;
    [SerializeField] AnimationCurve curve;

    new void Start()
    {
        base.Start();
        m_state = MimicState.Inactive;
        m_detectionZone = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        path = new NavMeshPath();
    }

    void Update()
    {
        
        switch (m_state)
        {
            case MimicState.Inactive:
                break;
            case MimicState.Active:
                ActiveBehaviour();
                break;
            case MimicState.Attacking:
                AttackingBehaviour();
                break;
        }
    }

    [SerializeField] float singleStep;
    [SerializeField] float maxMagnituteDelta;
    [SerializeField] float acceptableAngle;
    float elapsedStep;
    [SerializeField] float stepTime;
    [SerializeField] NavMeshPath path;
    
    private void ActiveBehaviour()
    {
        playerRef = charManager.GetCurrentCharacter();

        agent.SetDestination(playerRef.transform.position);
        elapsedStep += Time.deltaTime;
        agent.Move((agent.speed * Time.deltaTime * transform.forward) * curve.Evaluate(Mathf.Lerp(0f,1f,elapsedStep/stepTime)));
        if (elapsedStep > stepTime) 
        {
            elapsedStep = 0f;
            agent.CalculatePath(playerRef.transform.position, path);
        } 

        Vector3 movementDiretion = path.corners[1] - transform.position;
        movementDiretion.y = transform.position.y;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDiretion, singleStep * Time.deltaTime, maxMagnituteDelta);
        transform.rotation = Quaternion.LookRotation(newDirection);

        if (agent.remainingDistance < agent.stoppingDistance && Vector3.Angle(transform.forward, playerRef.transform.position - transform.position) < acceptableAngle)
        {
            m_state = MimicState.Attacking;
            _myAnimator.SetTrigger("Attack");
            elapsedStep = 0f;
        }
    }

    bool attacking;
    [SerializeField] Vector3 boxHalfExtends;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject boneCenter;
    [SerializeField] LayerMask playerMask;
    private void AttackingBehaviour()
    {
        if (attacking)
        {
            Collider[] cols = Physics.OverlapBox(boneCenter.transform.position + (transform.forward * offset.z) + (transform.right * offset.x), boxHalfExtends,transform.rotation, playerMask);
            if(cols.Length > 0)
            {
                foreach (Collider col in cols)
                {
                    if(col.gameObject.CompareTag("Player"))
                        col.gameObject.GetComponent<HurtBox>().TakeDamage(2,gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(boneCenter.transform.position + (transform.forward * offset.z) + (transform.right * offset.x) + (transform.up * offset.y), boxHalfExtends * 2);
    }

    public void StartAttack()
    {
        attacking = true;
    }
    
    public void EndAttack()
    {
        attacking = false;
    }

    public void FinishAttackAnimation()
    {
        m_state = MimicState.Active;
        agent.CalculatePath(playerRef.transform.position, path);
    }

    public override void TakeDamage(int damage)
    {
        _myAnimator.SetTrigger("Hit");
        m_state = MimicState.Inactive;
        transform.LookAt( new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z) );
        StartCoroutine(KnockbackDelay());

        base.TakeDamage(damage);
    }

    private System.Collections.IEnumerator KnockbackDelay()
    {
        //yield return new WaitForSeconds(0.1f);
        _myAnimator.speed = 0f;
        yield return new WaitForSeconds(0.15f);

        _myAnimator.speed = 1f;
        StartCoroutine(KnockbackCoroutine());
    }

    public UnityEvent _dieEvent;
    public override void Die()
    {
        _myAnimator.SetTrigger("Dead");
        _dieEvent.Invoke();
    }

    [SerializeField] AnimationCurve knockBackCurve;
    [SerializeField] float knockbackTime;
    [SerializeField] float knockbackForce;
    System.Collections.IEnumerator KnockbackCoroutine()
    {
        float elapsedCoroutineTime = 0;
        while(elapsedCoroutineTime < knockbackTime)
        {
            agent.Move( knockBackCurve.Evaluate(elapsedCoroutineTime / knockbackTime) * Time.deltaTime * (transform.forward * -knockbackForce));
            elapsedCoroutineTime += Time.deltaTime;
            yield return null;
        }
    }

    public void FinishHitAnimation()
    {
        if(curHealthAmount > 0)
        {
            m_state = MimicState.Active;
            agent.CalculatePath(playerRef.transform.position, path);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            charManager = CharacterManager.Instance;
            playerRef = other.gameObject;
            _myAnimator.SetBool("Active",true);
            m_detectionZone.enabled = false;
            m_state = MimicState.Active;
            agent.CalculatePath(playerRef.transform.position, path);
            foreach (var corner in path.corners)
            {
                Debug.DrawRay(corner, Vector3.up * 100f,Color.red, 1f);
            }
        }
    }

    public override void DestroyMe()
    {
        StopAllCoroutines();

        base.DestroyMe();
    }
}
