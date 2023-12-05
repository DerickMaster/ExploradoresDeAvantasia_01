using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : EnemyBehaviour
{
    public GameObject[] _slimeSpots;
    public int _currentTarget = 0;
    public float _step;
    public bool debugAtkRange;
    [HideInInspector] public bool _acting = false;
    // Update is called once per frame
    void Update()
    {
        if (!_acting)
        {
            transform.LookAt(_slimeSpots[_currentTarget].transform.position);
            transform.position = Vector3.MoveTowards(transform.position, _slimeSpots[_currentTarget].transform.position, _step * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, _slimeSpots[_currentTarget].transform.position) < 0.001f)
        {
            _currentTarget++;
            if (_currentTarget > _slimeSpots.Length-1)
            {
                _currentTarget = 0;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        _acting = true;
        _myAnimator.Play("Hit");
        base.TakeDamage(damage);
        Debug.Log("Ai");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _acting = true;
            _myAnimator.SetBool("Attack", true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(debugAtkRange)
            Gizmos.DrawSphere(transform.position, _damageRadius);
    }

    public float _damageRadius = 5f;
    void CheckSurroundings()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _damageRadius);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if(collider.CompareTag("Player"))
                    collider.GetComponent<HurtBox>().TakeDamage(2,gameObject);
            }
        }
    }

    [HideInInspector] public bool _attacking = false;
    private IEnumerator Attack()
    {
        _attacking = true;
        while (_attacking)
        {
            CheckSurroundings();
            yield return null;
        }
    }

    public void StartAttacking()
    {
        StartCoroutine(Attack());
    }

    public void StopAttacking()
    {
        _attacking = false;
    }

    public override void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void ReturnToWalk()
    {
        _myAnimator.SetBool("Turn", false);
        _myAnimator.SetBool("Hit", false);
        _myAnimator.SetBool("Attack", false);
        _acting = false;
    }
}
