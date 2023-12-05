using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauraAttackManager : AttackManager
{
    private Collider[] hitColliders;
    public Vector3 centerOffset;
    public float sphereRadius;
    public float distance;
    public int m_atkType;

    public int _damageModifier = 2;

    public Animator _specialAttack;

    public override void CheckHit()
    {
        StartCoroutine(Attacking());
    }

    private IEnumerator Attacking()
    {
        while (attacking)
        {
            hitColliders = Physics.OverlapSphere(gameObject.transform.position + centerOffset, sphereRadius, attackLayer);
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

    public void StartSpinCount()
    {
        StartCoroutine(SpinTime(_animator.GetFloat("HeldTime")));
    }

    Vector3 _attackHiddenPos;
    IEnumerator SpinTime(float duration)
    {
        _attackHiddenPos = _specialAttack.gameObject.transform.position;
        _specialAttack.gameObject.transform.position = gameObject.transform.position;

        yield return null;
        CheckHit();
        _specialAttack.SetBool("Active", true);
        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        myController.enabled = false;
        StopSpinAttack();
    }

    Coroutine spinCoroutine;
    public override void Unhold()
    {
        _animator.SetBool("Holding", false);
        if(triggered)
        {
            if (holdTime > 2f) holdTime = 2f;
            _animator.SetFloat("HeldTime", holdTime);
            spinCoroutine = StartCoroutine(SpinTime(holdTime));
        }
        holdTime = 0f;
    }

    public override void DisableAttack()
    {
        if(spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
            StopSpinAttack();
            spinCoroutine = null;
        }

        StopCheckingHit();
        myController.enabled = true;
        triggered = false;
        if (player_input.currentActionMap.name.Equals("PlayerAttacking")) player_input.SwitchCurrentActionMap("Player");
    }

    public override void StopCheckingHit()
    {
        attacking = false;
    }

    private void StopSpinAttack()
    {
        _animator.SetFloat("HeldTime", 0f);
        _specialAttack.SetBool("Active", false);
        _specialAttack.transform.position = _attackHiddenPos;
    }

    private bool triggered = false;

    public void CheckHold()
    {
        if ( holdTime > 0.1f)
        { 
            _animator.SetBool("Holding", true);
            triggered = true;
        }
    }
}