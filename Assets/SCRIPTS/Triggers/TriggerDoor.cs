using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : TriggerZone
{

    public DoorBehaviour doorReference;

    private bool referenceState = true;
    public bool happensOnce = false;

    

    public override void Triggered(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            doorReference.OpenDoor();
            referenceState = !referenceState;
        }
        if (happensOnce)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
