using UnityEngine;
using UnityEngine.Events;

public class BossVineBehaviour : EnemyBehaviour
{
    private HurtBox m_hurtBox;
    [HideInInspector] public UnityEvent destroyedEvent;

    private void Awake()
    {
        m_hurtBox = gameObject.GetComponentInChildren<HurtBox>();
    }

    public void SetOpenHurtbox(bool open)
    {
        m_hurtBox.SetOpenHurtbox(open);
    }

    public override void Die()
    {
        m_hurtBox.SetOpenHurtbox(false);
        _myAnimator.SetBool("Active", false);
    }

    public override void DestroyMe() { }
    public override void TakeDamage(int damage)
    {
        _myAnimator.Play("Hit");
        curHealthAmount -= damage;
        if (curHealthAmount <= 0)
        {
            destroyedEvent.Invoke();
            Die();
        }
    }

    public void ResetState()
    {
        curHealthAmount = maxHealthAmount;
        m_hurtBox.SetOpenHurtbox(true);
        _myAnimator.SetBool("Active", true);
    }
}