using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineVesselBehaviour : EnemyBehaviour
{
    public DoorBehaviour _myDoor;
    // Start is called before the first frame update

    public override void TakeDamage(int damage)
    {
        _myAnimator.Play("Hit");
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        _myDoor.OpenDoor();
    }
    /*
    public override void DestroyMe()
    {
        Destroy(gameObject);
    }
    */
    public void ResetHit()
    {
        _myAnimator.SetBool("Hit", false);
    }
}
