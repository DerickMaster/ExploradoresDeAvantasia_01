using System.Collections;
using UnityEngine;

public class ChestObjectBehaviour : SlottableObjectBehaviour
{
    [SerializeField] protected int qtObjectsExpected = 0;
    [SerializeField] protected int currentQtObject = 0;

    public InteractableObject reference;

    Animator myAnimator;
    protected new void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
        SetActiveOutline(false);
    }

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == keyObjectID)
        {
            GrabbableObjectInteractions.SwallowItem(interactor);
            PlayAnimation();
            currentQtObject++;
            if (currentQtObject == qtObjectsExpected)
            {
                reference.Interact(interactor);
            }
        }
    }

    public void PlayAnimation()
    {
        myAnimator.SetBool("Active", true);
        StartCoroutine(CooldownAnimation());
    }

    private IEnumerator CooldownAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetBool("Active", false);
    }
}
