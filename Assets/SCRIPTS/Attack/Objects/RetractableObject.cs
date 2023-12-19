using System.Collections;
using UnityEngine;

public class RetractableObject : EnemyBehaviour
{
    public override void TakeDamage(int damage)
    {
        StartCoroutine(RebuildCooldown());
    }

    [SerializeField] float _rebuildCooldown = 5f;
    private IEnumerator RebuildCooldown()
    {
        DeactivateCollider();
        yield return new WaitForSeconds(_rebuildCooldown);
        ActivateCollider();
    }

    [SerializeField] Collider _myCollider;
    public void ActivateCollider()
    {
        _myAnimator.SetBool("Active", false);
        _myCollider.enabled = true;
    }

    public void DeactivateCollider()
    {
        _myAnimator.SetBool("Active", true);
        _myCollider.enabled = false;
    }
}
