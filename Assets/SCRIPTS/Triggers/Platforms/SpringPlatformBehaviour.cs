using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPlatformBehaviour : TriggerZone
{
    Animator _myAnim;
    public float _knockHeight;
    public float _knockForce = 10.0f;
    public float _knockbackTime = 1.5f;
    public Vector3 _direction;
    bool _inCooldown = false;

    private void Start()
    {
        _myAnim = GetComponent<Animator>();
    }

    public override void Triggered(Collider other)
    {
        if(!_inCooldown)
            StartCoroutine(ThrowChar(other));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            Triggered(other);
    }

    private IEnumerator ThrowChar(Collider _char)
    {
        _inCooldown = true;
        _char.GetComponent<StarterAssets.ThirdPersonController>().TakeKnockback((_direction),_knockHeight,_knockForce,_knockbackTime);
        _myAnim.SetTrigger("Activated");
        yield return new WaitForSeconds(2f);
        _inCooldown = false;
    }
}
