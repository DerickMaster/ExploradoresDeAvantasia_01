using System.Collections;
using UnityEngine;

public class NPCCaramelDog : InteractableObject
{
    Animator _myAnimator;
    private new void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }

    bool _dancing = false;
    public override void Interact(InteractionController interactor)
    {
        if(!_dancing)
            StartCoroutine(DanceCD());
    }  

    private IEnumerator DanceCD()
    {
        _dancing = true;
        _myAnimator.SetBool("Dance", _dancing);
        yield return new WaitForSeconds(5f);
        _dancing = false;
        _myAnimator.SetBool("Dance", _dancing);
    }
}
