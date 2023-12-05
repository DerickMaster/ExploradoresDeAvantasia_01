using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFlowerTrigger : TriggerZone
{
    Animator _myAnimator;
    FreezingGroundBehaviour _myGround;


    private void Start()
    {
        _myAnimator = GetComponentInParent<Animator>();
        _myGround = GetComponentInChildren<FreezingGroundBehaviour>();
    }

    public override void Triggered(Collider other)
    {
        _myAnimator.SetTrigger("Active");
        _myGround.Freeze(2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) Triggered(other);
    }
}
