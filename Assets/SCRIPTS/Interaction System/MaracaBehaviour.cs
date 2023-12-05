using UnityEngine;
using UnityEngine.Events;

public class MaracaBehaviour : MonoBehaviour
{
    Animator m_animator;
    [HideInInspector] public UnityEvent shakeFinishedEvent;

    public void Initialize()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Shake()
    {
        m_animator.SetTrigger("Activate");
    }

    public void ShakeFinished()
    {
        shakeFinishedEvent.Invoke();
    }
}
