using System.Collections;
using UnityEngine;

public class LauraGear : InteractableObject
{
    public GameObject referenceInte;
    public Animator myAnimator;
    public LauraCable m_Cable;
    public float myTime = 0.5f;
    private void Awake()
    {
        m_Cable = GetComponentInChildren<LauraCable>();
    }
    public override void Interact(InteractionController interactor)
    {
        interactor.SpecialInteraction();
        referenceInte.GetComponent<InteractableObject>().Interact(interactor);
        StartCoroutine(WaitToActivate(myTime));

        try
        {
            GetComponentInChildren<LauraCable>().Activate();
        }
        catch (System.NullReferenceException)
        {
            Debug.Log(gameObject.name + "| Sem cabo");
        } 

        if (singleInteraction) DeactivateInteractable();
    }

    private IEnumerator WaitToActivate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        myAnimator.SetBool("Active", true);
    }

    public void RactivateGear()
    {
        ReactivateInteractable();
        myAnimator.SetBool("Active", false);
        if (m_Cable) m_Cable.Deactivate();
    }
}
