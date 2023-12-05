using UnityEngine;

public class BiaAttackManager : AttackManager
{
    [SerializeField] GameObject biaAtkPrefab;
    private GameObject biaAtkRef;
    public AttackColliderChecker m_atkCollider;
    private Animator attackAnim;
    private string atkType = "Normal";
    [SerializeField] float chargeTime;
    new void Start()
    {
        base.Start();
        biaAtkRef = Instantiate(biaAtkPrefab);
        m_atkCollider = biaAtkRef.GetComponentInChildren<AttackColliderChecker>();
        m_atkCollider.Initialize(this);
        attackAnim = biaAtkRef.GetComponentInChildren<Animator>();
        attackAnim.gameObject.SetActive(false);
    }

    public void CheckHold()
    {
        if(holdTime > 0f)
        {
            _animator.SetBool("Holding", true);
        }
    }

    public override void Unhold()
    {
        _animator.SetBool("Holding", false);
        if (holdTime > chargeTime) 
        {
            atkType = "Super";
            m_atkCollider.colId = 1;
        } 
        else
        {
            atkType = "Normal";
            m_atkCollider.colId = 0;
        }

        holdTime = 0f;
    }

    public override void CheckHit()
    {
        m_atkCollider.SetActiveCollider(true);
        attackAnim.gameObject.SetActive(true);
        attackAnim.SetTrigger(atkType);
        biaAtkRef.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    public void StopHit()
    {
        Debug.Log("turnoff");
        m_atkCollider.SetActiveCollider(false);
    }

    public override void TargetFound(Collider hitCollider)
    {
        hitCollider.GetComponent<HurtBox>().TakeDamage(2);
    }

    public override void DisableAttack()
    {
        attacking = false;
        m_atkCollider.SetActiveCollider(false);
        attackAnim.gameObject.SetActive(false);
        if (player_input.currentActionMap.name.Equals("PlayerAttacking")) player_input.SwitchCurrentActionMap("Player");
        _animator.SetBool("Attack", false);
    }
}
