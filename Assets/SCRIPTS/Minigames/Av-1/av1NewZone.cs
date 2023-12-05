using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class av1NewZone : TriggerZone
{

    public override void Triggered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            av1minigamemanager._instance._newRegionCount++;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
