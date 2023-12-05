using UnityEngine;
using UnityEngine.Events;

public class PlantBossBahaviour : MonoBehaviour
{
    private enum PlantBossState
    {
        Waiting,
        Activated,
        Attacking,
    } 

    private Animator m_animator;
    private Animator shot_Animator;
    public int curHp;

    [SerializeField] int maxHp;

    [HideInInspector] public UnityEvent activateDoTArea;
    [HideInInspector] public UnityEvent finishedAtk;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        shot_Animator = GetComponentsInChildren<Animator>()[1];
        materialController = GetComponentInChildren<ObjectMaterialController>();
        curHp = maxHp;
    }

    public void InitiateFight()
    {
        m_animator.SetTrigger("Started");
    }

    [ContextMenu("Attack")]
    public void DebugAttack()
    {
        PrepareAttack();
        UnleashAttack();
    }

    public void PrepareAttack()
    {
        m_animator.SetTrigger("Prepare");
    }
    
    public void ShootProjectile()
    {
        shot_Animator.SetTrigger("Shoot");
    }

    public void UnleashAttack()
    {
        m_animator.SetTrigger("Attack");
    }
    
    public void FinishAttack()
    {
        Debug.Log("Termino");
        finishedAtk.Invoke();
    }

    public void ActivateArea()
    {
        activateDoTArea.Invoke();
    }

    public void SetMaxHp(int newMaxHp)
    {
        maxHp = newMaxHp;
        curHp = maxHp;
    }

    [SerializeField] private Material damagedMat;
    protected ObjectMaterialController materialController;
    public void TakeHit()
    {
        m_animator.SetTrigger("Hit");
        if (damagedMat != null) materialController.AddMaterial(damagedMat);
        curHp--;
        if (curHp <= 0) 
        {
            enabled = false;
            m_animator.SetTrigger("Dead");
        } 
    }

    public void RemoveDamageMaterial()
    {
        materialController.RemoveMaterial(damagedMat.name);
    }
}