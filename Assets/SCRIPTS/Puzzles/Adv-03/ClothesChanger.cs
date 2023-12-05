using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesChanger : InteractableObject
{
    public GameObject target;
    [SerializeField] Material newMaterial;
    [SerializeField] Mesh newMesh;

    public override void Interact(InteractionController interactor)
    {
        if (!target) target = interactor.transform.parent.gameObject;
        target.GetComponentInChildren<SkinnedMeshRenderer>().material = newMaterial;
        target.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = newMesh;
    }
}
