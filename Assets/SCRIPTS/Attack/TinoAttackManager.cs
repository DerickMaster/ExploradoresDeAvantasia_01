using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinoAttackManager : AttackManager
{
    private Collider[] hitColliders;
    public Vector3 centerOffset;
    public float sphereRadius;
    public Vector3 boxHalfExtends;
    public float distance;
    public int m_atkType;

    Coroutine _holdCoroutine;

    private new void Start()
    {
        base.Start();
    }
    public override void StartAttack()
    {
        Debug.Log("Starting");

        attacking = true;
        player_input.SwitchCurrentActionMap("PlayerAttacking");
        _holdCoroutine = StartCoroutine(Holding());
    }

    private IEnumerator Attacking()
    {
        if(_charged) PlaceAttackEffect();
        while (attacking)
        {
            hitColliders = Physics.OverlapBox(gameObject.transform.position + centerOffset + (transform.forward * distance), boxHalfExtends, Quaternion.identity, attackLayer);
            if (hitColliders.Length > 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    collider.GetComponent<HurtBox>().TakeDamage(_attackModified);
                }
            }
            yield return null;
        }
        _attackModified = 2;
    }

    public GameObject _chargedEffectObj, _handBone;
    public void PlaceAttackEffect()
    {
        _chargedEffectObj.transform.position = _handBone.transform.position + transform.forward * 2;
        _chargedEffectObj.transform.rotation = gameObject.transform.rotation;
        _chargedEffectObj.GetComponent<Animator>().SetTrigger("Appear");
        _charged = false;
    }

    int _attackModified = 2;
    public GameObject _chargingParticle, _chargedParticle;
    bool _charged = false;
    private new IEnumerator Holding()
    {
        yield return null;

        float _timeElapsed = 0;
        if(_input.attackHold > 0)
        {
            _animator.SetBool("Holding", true);
            _chargingParticle.SetActive(true);
        }

        while (_input.attackHold > 0)
        {
            _timeElapsed += Time.deltaTime;
            _animator.SetFloat("HeldTime", _timeElapsed);

            if (_attackModified == 2 && _timeElapsed > 2)
            {
                _chargedParticle.SetActive(true);
                _chargingParticle.SetActive(false);

                _animator.SetBool("Charged", true);
                _charged = true;
                _attackModified = 6;
            }
            yield return null;
        }
        Unhold();
    }

    public override void Unhold()
    {
        //if(_holdCoroutine != null)
        //   StopCoroutine(_holdCoroutine);
        _animator.SetFloat("HeldTime", 0f);
        _animator.SetBool("Holding", false);

        _animator.SetBool("Attack", false);
        _animator.SetBool("Charge", false);
        _animator.SetBool("Charged", false);
        _chargingParticle.SetActive(false);
        _chargedParticle.SetActive(false);
    }

    public override void CheckHit()
    {
        attacking = true;
        StartCoroutine(Attacking());
    }

    public override void DisableAttack()
    {
        Debug.Log("Disabling");

        if (player_input.currentActionMap.name.Equals("PlayerAttacking")) player_input.SwitchCurrentActionMap("Player");
        _animator.SetBool("Attack", false);
        attacking = false;
    }
}
