using UnityEngine;

public class CircleWarningBehaviour : MonoBehaviour
{
    Animator m_animator;
    float elapsedTime = 0f;
    public float timeToFill;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        m_animator.SetFloat("FillAmount", elapsedTime / timeToFill);
        elapsedTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        if(!m_animator) m_animator = GetComponent<Animator>();
        m_animator.SetBool("Active", true);
    }

    private void OnDisable()
    {
        m_animator.SetBool("Active", false);
        elapsedTime = 0f;
    }
}
