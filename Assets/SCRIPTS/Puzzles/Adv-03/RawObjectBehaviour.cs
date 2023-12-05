using UnityEngine;

public class RawObjectBehaviour : GrabbableObject
{
    private Transform originalParent;

    private new void Start()
    {
        originalParent = transform.parent;
    }

    public void ReturnToOriginalParent()
    {
        gameObject.transform.SetParent(originalParent);
        gameObject.transform.localPosition = Vector3.zero;
        SetPhysics(true);
    }
}