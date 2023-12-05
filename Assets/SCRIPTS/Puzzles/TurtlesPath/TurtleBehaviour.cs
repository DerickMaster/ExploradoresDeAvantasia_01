using UnityEngine;

public class TurtleBehaviour : MonoBehaviour
{
    private Animator m_animator;
    public int Id;
    [SerializeField] Vector3 gizmosCube;

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
    }

    public void SetActivated(bool active)
    {
        m_animator.SetBool("Active", active);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.up * gizmosCube.y/2, gizmosCube);
    }
}