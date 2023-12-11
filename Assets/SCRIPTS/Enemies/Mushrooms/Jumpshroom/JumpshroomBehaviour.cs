using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpshroomBehaviour : EnemyBehaviour
{
    [SerializeField] Collider detectionZone;
    [SerializeField] GameObject hitboxOrigin;
    [SerializeField] HitboxData hitbox;

    bool active = false;
    Collider hurtboxCol;
    FollowPlayerBehaviour followPlayer;
    NavMeshAgent agent;
    NavMeshPath path;

    new void Start()
    {
        base.Start();
        followPlayer = GetComponent<FollowPlayerBehaviour>();
        hurtboxCol = GetComponentInChildren<HurtBox>().GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    void Update()
    {
        if (!active) return;

        if (jumping) JumpMovement();
        else AdjustDirection();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = true;
            detectionZone.enabled = false;
            _myAnimator.SetBool("Active", true);
        }
    }

    private float elapsedTime;
    private Vector3 targetPos;
    [SerializeField] private float lookDelay;
    private void AdjustDirection()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > lookDelay)
        {
            agent.CalculatePath(CharacterManager.Instance.GetCurrentCharacter().transform.position, path);
            elapsedTime = 0f;
        }
        targetPos = followPlayer.AdjustLookDirection(path);
    }

    public void StartMoving()
    {
        jumping = true;
        hurtboxCol.enabled = false;
    }

    bool jumping = false;
    [SerializeField] float jumpSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask mask;
    private void JumpMovement()
    {
        agent.Move(Mathf.Lerp(0f,1f, Vector3.Distance(transform.position, targetPos) / maxDistance) * jumpSpeed * Time.deltaTime * transform.forward);

        Vector3 pos = hitboxOrigin.transform.position + transform.up * hitbox.offset.y + transform.right * hitbox.offset.x + transform.forward * hitbox.offset.z;
        Collider[] cols = Physics.OverlapBox(pos, hitbox.bounds / 2, transform.rotation, mask);
        if(cols.Length > 0)
        {
            foreach (Collider col in cols)
            {
                if (col.CompareTag("Player")) col.GetComponent<HurtBox>().TakeDamage(2, gameObject);
                else if (col.GetComponent<Challenge_10.HeavyButtonListener>() is var btnListener && btnListener != null ) btnListener.Press();
            }
        }
    }

    public void StopMoving()
    {
        jumping = false;
        hurtboxCol.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = hitboxOrigin.transform.position + transform.up * hitbox.offset.y + transform.right * hitbox.offset.x + transform.forward * hitbox.offset.z;
        Gizmos.DrawCube(pos, hitbox.bounds);
    }
}
