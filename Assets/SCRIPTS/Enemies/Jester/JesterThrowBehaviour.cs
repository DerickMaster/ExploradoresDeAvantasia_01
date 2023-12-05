using System.Collections;
using UnityEngine;

public class JesterThrowBehaviour : MonoBehaviour
{
    private GameObject m_target;
    private GameObject projectile;
    private bool hasTarget = false;
    private Animator m_Anim;
    private Coroutine disappearArrowCoroutine;

    [SerializeField] private GameObject warningArrow;
    [SerializeField] private GameObject attackOrigin;
    [SerializeField] private GameObject projectilePrefab;

    void Start()
    {
        m_target = GameObject.FindGameObjectWithTag("Player");
        m_Anim = GetComponent<Animator>();
        projectile = Instantiate(projectilePrefab);
        projectile.SetActive(false);
        warningArrow.SetActive(false);
    }

    Vector3 direction = Vector3.zero;
    Quaternion rotation;
    void Update()
    {
        if (hasTarget)
        {
            direction = m_target.transform.position - gameObject.transform.position;
            rotation = Quaternion.LookRotation(direction);
            rotation.x = 0f;
            rotation.z = 0f;
            gameObject.transform.rotation = rotation;
        }
    }

    [ContextMenu("Activate")]
    public void ActivateShooting()
    {
        hasTarget = true;
        m_Anim.SetTrigger("Attack");
        warningArrow.SetActive(true);
        if(disappearArrowCoroutine != null)
        {
            StopCoroutine(disappearArrowCoroutine);
            disappearArrowCoroutine = null;
        }
    }

    public void ShootProjectile()
    {
        projectile.SetActive(false);
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.x = 0f; rotation.z = 0f;
        projectile.transform.SetPositionAndRotation(attackOrigin.transform.position, rotation);
        projectile.SetActive(true);
        hasTarget = false;
        disappearArrowCoroutine = StartCoroutine(DisappearArrowDelay(2f));
    }

    IEnumerator DisappearArrowDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        warningArrow.SetActive(false);
        disappearArrowCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, direction);
    }
}