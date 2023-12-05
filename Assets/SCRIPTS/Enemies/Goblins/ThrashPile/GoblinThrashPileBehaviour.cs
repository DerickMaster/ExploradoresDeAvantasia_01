using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinThrashPileBehaviour : MonoBehaviour
{
    private DetectionArea m_area;
    private GameObject m_target;
    private Animator m_animator;
    private Animator _goblinAnimator;
    [SerializeField] private GameObject _goblinModel;
    [SerializeField] private GameObject warningArrow;

    public GameObject hand;
    public GameObject projectilePrefab;

    private bool _hasTarget = false;

    private LinkedList<GameObject> projectiles;
    private LinkedListNode<GameObject> curProjectile;
    [SerializeField] private int projectileAmount;
    [SerializeField] private bool continuous;
    [SerializeField] private float attackDelay;

    private void Start()
    {
        m_area = GetComponentInChildren<DetectionArea>();
        Animator[] animators = GetComponentsInChildren<Animator>();
        m_animator = animators[0];
        _goblinAnimator = animators[1];

        m_area.PlayerDetected.AddListener(GetTarget);
        m_area.PlayerLost.AddListener(LostTarget);

        projectiles = new LinkedList<GameObject>();
        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform);
            projectiles.AddLast(projectile);
            projectile.SetActive(false);

        }
        curProjectile = projectiles.First;
        GetComponentInChildren<GoblinTPEventReceiver>().shootEvent.AddListener(ShootProjectile);
    }

    Quaternion rotation;
    private void Update()
    {
        if (_hasTarget)
        {
            direction = m_target.transform.position - hand.transform.position;
            rotation = Quaternion.LookRotation(direction);
            rotation.x = 0f;
            rotation.z = 0f;
            _goblinModel.transform.rotation = rotation;
            //warningArrow.transform.rotation = rotation * Quaternion.Euler(Vector3.up * 180);
        }
    }

    private void GetTarget()
    {
        _hasTarget = true;
        m_target = m_area.player;
        m_animator.SetBool("Active", true);
        _goblinAnimator.SetBool("Active", true);
        //warningArrow.SetActive(true);
    }

    private void LostTarget()
    {
        _hasTarget = false;
        m_animator.SetBool("Active", false);
        _goblinAnimator.SetBool("Active", false);
        warningArrow.SetActive(false);
    }

    Vector3 direction = Vector3.zero;
    public void ShootProjectile()
    {
        curProjectile = curProjectile.Next ?? projectiles.First;
        curProjectile.Value.SetActive(false);

        direction = m_target.transform.position - hand.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.x = 0f; rotation.z = 0f;

        warningArrow.SetActive(true);
        warningArrow.transform.rotation = rotation * Quaternion.Euler(Vector3.up * 180);

        curProjectile.Value.transform.rotation = rotation;

        curProjectile.Value.transform.position = hand.transform.position;
        curProjectile.Value.SetActive(true);

        if (!continuous)
        {
            StartCoroutine(AttackDelay());
        }
    }

    private IEnumerator AttackDelay()
    {
        m_animator.SetBool("Active", false);
        _goblinAnimator.SetBool("Active", false);

        yield return new WaitForSeconds(attackDelay);

        if (_hasTarget)
        {
            m_animator.SetBool("Active", true);
            _goblinAnimator.SetBool("Active", true);
        }
    }

}
