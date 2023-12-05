using UnityEngine;

public class BookPlatformBehaviour : MonoBehaviour
{
    private Animator m_Animator;
    private ActivationBookBehaviour m_Book;
    [SerializeField]private Collider m_Col;
    [SerializeField]private Collider wall_Col;

    private void Start()
    {
        m_Col = GetComponent<Collider>();
        m_Col.enabled = false;
        m_Animator = GetComponent<Animator>();
        m_Book = transform.parent.GetComponentInChildren<ActivationBookBehaviour>();
        m_Book.bookInteractedEvent.AddListener(SwitchState);
    }

    private void SwitchState(ActivationBookBehaviour book, bool state, bool playerInteracted)
    {
        m_Animator.SetBool("Raised", state);
        m_Col.enabled = state;
        if (wall_Col) wall_Col.enabled = !state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, m_Col.bounds.extents*2);
    }
}
