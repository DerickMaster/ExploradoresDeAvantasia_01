using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiaLockerBehaviour : InteractableObject
{
    Animator _myAnimator;

    new
        // Start is called before the first frame update
        void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }


    public GameObject _specialGate;
    public override void Interact(InteractionController interactor)
    {
        _myAnimator.SetBool("Active", true);
        int nameToLayer = LayerMask.NameToLayer("Interactable");
        _specialGate.layer = nameToLayer;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
