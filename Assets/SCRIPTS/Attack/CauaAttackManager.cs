
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CauaAttackManager : AttackManager
{
    private Collider[] hitColliders;
    private Coroutine _holdCoroutine;
    private StarterAssets.ActionsAllowed actions;
    private CharacterController controller;
    private PlayerInput p_Input;

    public Vector3 centerOffset;
    public float sphereRadius;
    public Vector3 boxHalfExtends;
    public float distance;
    public int m_atkType;
    public bool localKeepAttacking = false;
    public float dashSpeed;
    public float maxDashDuration;
    public AnimationCurve curve;

    public Animator _specialAttack;

    private void Update()
    {
        Attack();
        Debug.DrawRay(transform.position, transform.forward.normalized * 2f, Color.green);
    }

    private void Awake()
    {
        p_Input = GetComponent<PlayerInput>();
        controller = GetComponentInParent<CharacterController>();
        actions = new StarterAssets.ActionsAllowed
        {
            canMove = false,
            canAttack = true,
            canInteract = false,
            canJump = false,
            canSwitch = false
        };
    }

    private new void Attack()
    {
        if (_input.attack)
        {
            _input.attack = false;
            attackEvent.Invoke();
            if (myController.Grounded && !interactor.HoldingObject)
            {
                _animator.SetTrigger("Attack");
            }
        }
    }

    //[SerializeField] private float holdTime;
    private float dashTime;

    public override void Unhold()
    {
        dashTime = holdTime/4;
        if (dashTime > maxDashDuration) dashTime = maxDashDuration;
        holdTime = 0f;
        _animator.SetBool("Holding", false);
    }

    public override void StartAttack()
    {
        //actionsBlockEvent.Invoke(actions);
        //myController.BlockActions(actions);

        player_input.SwitchCurrentActionMap("PlayerAttacking");

        StartCoroutine(Holding());
    }

    public void CheckAttackType()
    {
        if(holdTime > 0f)
        {
            _animator.SetBool("Holding", true);
        }
    }

    public void StartDash()
    {
        //p_Input.enabled = false;
        p_Input.DeactivateInput();

        GetComponent<HurtBox>().SetOpenHurtbox(false);

        StartCoroutine(DashMovement());
        CheckHit();
    }

    Vector3 _attackHiddenPos;
    private IEnumerator DashMovement()
    {
        myController.ChangeMoveSpeed(dashSpeed);

        _attackHiddenPos = _specialAttack.gameObject.transform.position;
        _specialAttack.gameObject.transform.position = gameObject.transform.position;
        _specialAttack.SetTrigger("Appear");
        
        Vector3 direction = transform.forward;
        float elapsedTime = 0f;

        while(elapsedTime < dashTime)
        {
            _input.move.x = direction.x;
            _input.move.y = direction.z;
            myController.ChangeMoveSpeed(dashSpeed * curve.Evaluate(elapsedTime/dashTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _input.move.x = 0;
        _input.move.y = 0;

        yield return new WaitForSeconds(0.2f);

        p_Input.ActivateInput();
        DisableAttack();
        GetComponent<HurtBox>().SetOpenHurtbox(true);
        _animator.SetTrigger("FinishDash");

        _specialAttack.gameObject.transform.position = _attackHiddenPos;
        myController.ResetMoveSpeed();
    }

    private IEnumerator Attacking()
    {
        _input.keepAttacking = false;
        bool singleTap = true;
        while (attacking)
        {
            if (_input.keepAttacking && singleTap) 
            {
                singleTap = false;
                localKeepAttacking = true;
                _input.keepAttacking = false;
                _animator.SetTrigger("Attack");
            }

            hitColliders = Physics.OverlapBox(gameObject.transform.position + centerOffset + (transform.forward * distance), boxHalfExtends, Quaternion.identity, attackLayer);
            if (hitColliders.Length > 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    collider.GetComponent<HurtBox>().TakeDamage(2);
                }
            }
            yield return null;
        }
    }

    public override void CheckHit()
    {
        attacking = true;
        StartCoroutine(Attacking());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.DrawSphere(gameObject.transform.position + centerOffset, sphereRadius);
        Gizmos.DrawCube(centerOffset + (transform.forward * distance), boxHalfExtends * 2);
    }

    public override void DisableAttack()
    {
        if (localKeepAttacking) myController.enabled = false;
        else 
        {
            myController.enabled = true;
        }
        ResetValues();
    }

    public void FinalAttack()
    {
        myController.enabled = true;
        ResetValues();
    }

    private void ResetValues()
    {
        if (!localKeepAttacking && player_input.currentActionMap.name.Equals("PlayerAttacking")) player_input.SwitchCurrentActionMap("Player");
        attacking = false;
        localKeepAttacking = false;
        _specialAttack.SetTrigger("Disappear");
        //_animator.SetBool("Attack", false);
    }
}