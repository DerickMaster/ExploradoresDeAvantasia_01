using UnityEngine;
using UnityEngine.Events;

public class CircleWarningBehaviour : MonoBehaviour
{
    Animator m_animator;
    float elapsedTime = 0f;
    float height;

    public GameObject target;
    public float timeToFill;
    [HideInInspector] public UnityEvent finishedFillingEvent;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        m_animator.SetFloat("FillAmount", elapsedTime / timeToFill);
        elapsedTime += Time.deltaTime;

        if (elapsedTime > timeToFill) finishedFillingEvent.Invoke();

        if (target) FollowTarget();
    }

    public void FollowTarget()
    {
        Vector3 pos = target.transform.position;
        pos.y = height;

        transform.position = pos;
    }

    private void OnEnable()
    {
        if(!m_animator) m_animator = GetComponent<Animator>();
        m_animator.SetBool("Active", true);
        height = transform.position.y;
    }

    private void OnDisable()
    {
        m_animator.SetBool("Active", false);
        elapsedTime = 0f;
    }
}
