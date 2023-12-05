using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    [SerializeField] float testSize;
    [SerializeField] LayerMask playerMask;
    [SerializeField] Animator m_Animator;

    public void PlayExplosion(int dmg = 2, float size = 1f)
    {
        m_Animator.SetTrigger("Explode");
        transform.localScale = Vector3.one * size;

        if(dmg > 0)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, size, playerMask);
            if (cols.Length > 0)
            {
                foreach (Collider col in cols)
                {
                    col.GetComponent<HurtBox>().TakeDamage(dmg, gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, testSize);
    }
}
