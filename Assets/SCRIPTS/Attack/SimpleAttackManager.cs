using System.Collections;
using UnityEngine;

public class SimpleAttackManager : AttackManager
{
    private Collider[] hitColliders;
    public Vector3 centerOffset;
    public float sphereRadius;
    public Vector3 boxHalfExtends;
    public float distance;
    public int m_atkType;

    public override void StartAttack()
    {
        attacking = true;
        player_input.SwitchCurrentActionMap("PlayerAttacking");
        StartCoroutine(Attacking());
    }

    public int _damageModifier = 2;
    private IEnumerator Attacking()
    {
        while (attacking)
        {
            hitColliders = CheckAttackType();
            if (hitColliders.Length > 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    collider.GetComponent<HurtBox>().TakeDamage(_damageModifier);
                }
            }
            yield return null;
        }
    }

    private Collider[] CheckAttackType()
    {
        return m_atkType switch
        {
            0 => Physics.OverlapSphere(gameObject.transform.position + centerOffset, sphereRadius, attackLayer),
            1 => Physics.OverlapBox(gameObject.transform.position + centerOffset + (transform.forward * distance), boxHalfExtends, Quaternion.identity, attackLayer),
            _ => null,
        };
    }

    public override void DisableAttack()
    {
        if (player_input.currentActionMap.name.Equals("PlayerAttacking")) player_input.SwitchCurrentActionMap("Player");
        _animator.SetBool("Attack", false);
        attacking = false;
    }
}
