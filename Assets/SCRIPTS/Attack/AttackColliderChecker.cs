using UnityEngine;

public class AttackColliderChecker : MonoBehaviour
{
    private AttackManager m_attackManager;
    private Collider[] m_Colliders;
    public int colId = 0;

    public void Initialize(AttackManager atkManager)
    {
        m_attackManager = atkManager;
        m_Colliders = GetComponents<MeshCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        m_attackManager.TargetFound(other);
    }

    public void SetActiveCollider(bool active)
    {
        m_Colliders[colId].enabled = active;
    }
}