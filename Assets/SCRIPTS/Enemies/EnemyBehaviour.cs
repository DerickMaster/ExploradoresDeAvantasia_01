using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviour : HealthUnit
{
    [HideInInspector] public Animator _myAnimator;
    [HideInInspector] public UnityEvent unitKilled;
    [SerializeField] protected Material damagedMat;
    protected ObjectMaterialController materialController;

    protected void Start()
    {
        _myAnimator = GetComponent<Animator>();
        curHealthAmount = maxHealthAmount;
        materialController = GetComponentInChildren<ObjectMaterialController>();
    }

    public override void TakeDamage(int damage)
    {
        if(damagedMat != null) materialController.AddMaterial(damagedMat);
        base.TakeDamage(damage);
        if (curHealthAmount == 0)
        {
            Die();
        }
    }

    public void RemoveDamagedMaterial()
    {
        materialController.RemoveMaterial(damagedMat.name);
    }

    public virtual void Die()
    {
        _myAnimator.Play("Death");
        unitKilled.Invoke();
    }

    public virtual void DestroyMe()
    {
        Destroy(gameObject);
    }
}
