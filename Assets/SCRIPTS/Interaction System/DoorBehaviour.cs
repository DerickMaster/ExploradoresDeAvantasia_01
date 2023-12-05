using System.Collections;
using UnityEngine;

public class DoorBehaviour : InteractableObject
{
    Animator myAnimator;
    public Animator _switchAnimator;
    public Collider myCollider;
    [SerializeField] private bool open = false;

    private new void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponentInChildren<Collider>();

        if (!open) CloseDoor(); else OpenDoor();
    }

    public override void Interact(InteractionController interactor)
    {
        OpenDoor();
    }

    public override void Interact(InteractionController interactor, bool option)
    {
        if (option && !open) OpenDoor();
        else if(!option && open) CloseDoor();
    }

    [ContextMenu("Abrir")]
    public void OpenDoor()
    {
        StartCoroutine(DelayOpenDoor());
        open = true;
    }

    public float _delayTime = 1f;
    private IEnumerator DelayOpenDoor()
    {
        yield return new WaitForSeconds(_delayTime);
        myAnimator.SetBool("Active", true);
        myCollider.enabled = false;
    }

    [ContextMenu("Fechar")]
    public void CloseDoor()
    {
        if (_switchAnimator != null)
        {
            _switchAnimator.gameObject.GetComponent<LauraGear>().RactivateGear();
        }
        myAnimator.SetBool("Active", false);
        myCollider.enabled = true;
        open = false;
    }
}