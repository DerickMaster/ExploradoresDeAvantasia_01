using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatUnit : MonoBehaviour
{
    [SerializeField] Attack[] attackList;
    [SerializeField] Dictionary<string, Attack> m_Moves;
    private bool attacking;

    private Animator m_Animator;
    private string[] chainToUse;
    private int chainLength = 0;
    private int curSequence;
    public UnityEvent<string> MoveUsedEvent { private set; get; }
    public UnityEvent MoveFinishedEvent { private set; get; }

    private void Awake()
    {
        MoveUsedEvent = new UnityEvent<string>();
        MoveFinishedEvent = new UnityEvent();
    }

    private void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Moves = new Dictionary<string, Attack>();

        foreach (Attack atk in attackList)
        {
            m_Moves.Add(atk.name, atk);
        }
    }

    [SerializeField] string moveName;
    [ContextMenu("Debug Use Move")]
    public void DebugUseMove()
    {
        UseMove(moveName);
    }

    
    public void UseChain(string[] chainToUse)
    {
        this.chainToUse = chainToUse;
        chainLength = chainToUse.Length;
        curSequence = 0;

        SetChaining(true);
        UseMove(chainToUse[curSequence], true);
    }

    public void UseMove(string moveToUse, bool chain = false)
    {
        if (m_Moves.ContainsKey(moveToUse))
        {
            moveName = moveToUse;

            CheckHitBox();
            attacking = true;

            if(!chain) m_Animator.SetTrigger("Attacking");
            m_Animator.SetTrigger(moveToUse);
            MoveUsedEvent.Invoke(moveToUse);
        }
        else Debug.Log("Nao existe o ataque de nome: " + moveToUse);
    }

    private void SetChaining(bool active)
    {
        if(active) m_Animator.SetTrigger("Attacking");
        m_Animator.SetBool("Chain", active);
    }

    private Attack atk;
    [ContextMenu("Check Hitbox")]
    public void CheckHitBox()
    {
        if (m_Moves.TryGetValue(moveName, out atk)) return;
        //Debug.Log("Ataque encontrado");
        else Debug.Log("Ataque nao encontrado");
    }

    [SerializeField] LayerMask playerLayerMask;
    private IEnumerator CheckAttackArea()
    {
        Collider[] found;
        while (attacking)
        {
            found = Physics.OverlapBox(atk.originObj.transform.position + atk.hitbox.offset, atk.hitbox.bounds / 2, Quaternion.identity, playerLayerMask);
            foreach(Collider col in found)
            {
                col.gameObject.GetComponentInChildren<HurtBox>().TakeDamage(2);
            }
            yield return null;
        }
    }

    public void CheckHit()
    {
        StartCoroutine(CheckAttackArea());
    }

    public void FinishCheckHit()
    {
        MoveFinishedEvent.Invoke();
        attacking = false;
        CheckEndAttack();
    }

    private void CheckEndAttack()
    {
        curSequence++;
        if (curSequence < chainLength)
        {
            UseMove(chainToUse[curSequence], true);
        }
        else
        {
            SetChaining(false);
            chainLength = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (m_Moves != null && atk != null)
        {
            Gizmos.DrawCube(atk.originObj.transform.position + atk.hitbox.offset, atk.hitbox.bounds);
        }
    }
}
