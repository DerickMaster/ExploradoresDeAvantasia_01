using UnityEngine;

public class LauraCable : MonoBehaviour
{
    public Material _deactivated;
    public Material _activated;

    [ContextMenu("Activate")]
    public void Activate()
    {
        ChangeMaterial(_activated);
    }

    [ContextMenu("Deactivate")]
    public void Deactivate()
    {
        ChangeMaterial(_deactivated);
    }

    private void ChangeMaterial(Material newMat)
    {
        foreach (Renderer wire in GetComponentsInChildren<Renderer>())
        {
            wire.material = newMat;
        }
    }
}
