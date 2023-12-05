using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimationTrigger : TriggerZone
{
    public override void Triggered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Animator>().SetBool("Active", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Animator>().SetBool("Active", false);
        }
    }
}
