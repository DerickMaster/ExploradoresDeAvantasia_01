using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTriggerZone : TriggerZone
{
    [SerializeField] InteractableObject obj;
    public override void Triggered(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            obj.Interact(other.gameObject.GetComponentInChildren<InteractionController>());
            if (_happensOnce) Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
