using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassableObjects : TriggerZone
{
    public Animator myAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            activated();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            desactivated();
    }

    private void activated()
    {
        myAnimator.SetBool("active", true);
    }
    private void desactivated()
    {
        myAnimator.SetBool("active", false);
    }
}