using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BombTreeBehaviour : MonoBehaviour
{
    Animator _animator;
    [HideInInspector] public UnityEvent throwBombEvent;

    [SerializeField] private Material damagedMat;
    protected ObjectMaterialController materialController;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        materialController = GetComponentInChildren<ObjectMaterialController>();
    }

    [ContextMenu("Wake up")]
    public void WakeUp()
    {
        _animator.SetTrigger("Active");
    }

    [ContextMenu("Attack")]
    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void ThrowBombs()
    {
        throwBombEvent.Invoke();
    }

    public void TakeDamage()
    {
        StartCoroutine(TakeDamageCoroutine());
    }

    private IEnumerator TakeDamageCoroutine()
    {
        materialController.AddMaterial(damagedMat);
        yield return new WaitForSeconds(2f);
        materialController.RemoveMaterial(damagedMat.name);
    }
}