using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance { get; private set; }
    public StarterAssets.StarterAssetsInputs _input { get; private set; }
    public PlayerInput player_input { get; private set; }

    protected Animator _animator;

    public LayerMask attackLayer;

    public StarterAssets.ThirdPersonController myController;
    [SerializeField] protected InteractionController interactor;

    [HideInInspector] public UnityEvent attackEvent;
    public bool attacking = false;
    // Start is called before the first frame update
    protected void Start()
    {
        
        myController = GetComponent<StarterAssets.ThirdPersonController>();
        _input = GetComponent<StarterAssets.StarterAssetsInputs>();
        player_input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        interactor = GetComponentInChildren<InteractionController>();
        attackEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        CheckKeepAttack();
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.DrawSphere(gameObject.transform.position + centerOffset, sphereRadius);
        Gizmos.DrawCube(Vector3.zero + centerOffset + (transform.forward * distance), boxHalfExtends * 2);
    }
    */

    public void Attack()
    {
        if (_input.attack)
        {
            _input.attack = false;
            attackEvent.Invoke();
            if (!attacking && myController.Grounded && !interactor.HoldingObject)
            {
                _animator.SetTrigger("Attack");
                //StartAttack();
            }
        }
    }

    protected float holdTime;
    public virtual void StartAttack()
    {
        attacking = true;
        player_input.SwitchCurrentActionMap("PlayerAttacking");
        StartCoroutine(Holding());
    }

    protected IEnumerator Holding()
    {
        //Debug.Log("Checking hold");
        yield return null;

        while (_input.attackHold > 0)
        {
            holdTime += Time.deltaTime;
            HoldCheck();
            yield return null;
        }
        Unhold();
    }

    public virtual void Unhold() { Debug.Log("LET IT GOOOOOO"); }
    public virtual void CheckHit() { }
    public virtual void StopCheckingHit() { }
    public virtual void DisableAttack() { }
    public virtual void CheckKeepAttack() { }
    public virtual void TargetFound(Collider hitCollider) { }
    protected virtual void HoldCheck() { }
}
