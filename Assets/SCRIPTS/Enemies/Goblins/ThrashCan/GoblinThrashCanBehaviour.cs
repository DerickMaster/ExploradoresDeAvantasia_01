using UnityEngine;
using System.Collections;

public class GoblinThrashCanBehaviour : MonoBehaviour
{
    private Animator m_animator;
    private bool attacking = false;
    [SerializeField] private bool hasDectectionArea = true; 
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        if (hasDectectionArea)
        {
            GetComponentInChildren<DetectionArea>().PlayerDetected.AddListener(DetectedPlayer);
            GetComponentInChildren<DetectionArea>().PlayerLost.AddListener(LostPlayer);
        }
    }

    public void DetectedPlayer()
    {
        m_animator.SetBool("Active", true);
    }

    private void LostPlayer()
    {
        m_animator.SetBool("Active", false);
    }

    public void StartAttacking()
    {
        attacking = true;
        StartCoroutine(CheckArea());
    }

    Collider[] hitColliders;
    [SerializeField] Vector3 centerOffset;
    [SerializeField] float distance;
    [SerializeField] Vector3 boxHalfExtends;
    [SerializeField] LayerMask playerLayer;
    private IEnumerator CheckArea() 
    {
        while (attacking)
        {
            hitColliders = Physics.OverlapBox(gameObject.transform.position + centerOffset + (transform.forward * distance), boxHalfExtends, Quaternion.identity, playerLayer);
            if (hitColliders.Length > 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    collider.GetComponent<HurtBox>().TakeDamage(2, gameObject, 10);
                }
            }
            yield return null;
        }
    
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, transform.localScale);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(gameObject.transform.position + centerOffset + (transform.forward * distance), boxHalfExtends*2);
        // to visualize t$$anonymous$$s:
    }

    public void StopAttacking()
    {
        attacking = false;
    }
}