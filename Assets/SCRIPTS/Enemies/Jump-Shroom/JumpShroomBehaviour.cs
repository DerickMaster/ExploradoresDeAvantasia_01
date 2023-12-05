using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class JumpShroomBehaviour : EnemyBehaviour
{
    private enum ShroomState
    {
        Inactive,
        Active
    }

    ShroomState m_state;
    Collider m_detectionZone;
    NavMeshAgent agent;
    GameObject playerRef;
    CharacterManager charManager;
    [SerializeField] AnimationCurve curve;

    new void Start()
    {
        base.Start();
        m_state = ShroomState.Inactive;
        m_detectionZone = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        path = new NavMeshPath();
    }

    void Update()
    {

        switch (m_state)
        {
            case ShroomState.Inactive:
                break;
            case ShroomState.Active:
                ActiveBehaviour();
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
        agent.Move((agent.speed * Time.deltaTime * transform.forward) * curve.Evaluate(Mathf.Lerp(0f, 1f, elapsedStep / stepTime)));
        if (elapsedStep > stepTime)
        {
            elapsedStep = 0f;
            agent.CalculatePath(playerRef.transform.position, path);
        }

        Vector3 movementDiretion = path.corners[1] - transform.position;
        movementDiretion.y = transform.position.y;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDiretion, singleStep * Time.deltaTime, maxMagnituteDelta);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    [SerializeField] Vector3 boxHalfExtends;
    [SerializeField] Vector3 offset;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position + (transform.forward * offset.z) + (transform.right * offset.x) + (transform.up * offset.y), boxHalfExtends * 2);
    }

    public override void TakeDamage(int damage)
    {
        _myAnimator.SetTrigger("Hit");
        m_state = ShroomState.Inactive;
        transform.LookAt(new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z));
        StartCoroutine(KnockbackDelay());

        base.TakeDamage(damage);
    }

    private System.Collections.IEnumerator KnockbackDelay()
    {
        _myAnimator.speed = 0f;
        yield return new WaitForSeconds(0.15f);

        _myAnimator.speed = 1f;
        StartCoroutine(KnockbackCoroutine());
    }

    [HideInInspector]public UnityEvent _dieEvent;
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
        while (elapsedCoroutineTime < knockbackTime)
        {
            agent.Move(knockBackCurve.Evaluate(elapsedCoroutineTime / knockbackTime) * Time.deltaTime * (transform.forward * -knockbackForce));
            elapsedCoroutineTime += Time.deltaTime;
            yield return null;
        }
    }

    public void FinishHitAnimation()
    {
        if (curHealthAmount > 0)
        {
            m_state = ShroomState.Active;
            agent.CalculatePath(playerRef.transform.position, path);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            charManager = CharacterManager.Instance;
            playerRef = other.gameObject;
            _myAnimator.SetBool("Active", true);
            m_detectionZone.enabled = false;
            m_state = ShroomState.Active;
            agent.CalculatePath(playerRef.transform.position, path);
            foreach (var corner in path.corners)
            {
                Debug.DrawRay(corner, Vector3.up * 100f, Color.red, 1f);
            }
        }
    }

    public override void DestroyMe()
    {
        StopAllCoroutines();

        base.DestroyMe();
    }
}
