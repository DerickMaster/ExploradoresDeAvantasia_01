using UnityEngine;

public class MagicStoneChestBehaviour : MonoBehaviour
{
    Animator m_animator;
    RecipientBehaviour m_recipient;
    GameObject objToThrow;
    [SerializeField] float throwForce;
    [SerializeField] GameObject stonePoint;
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_recipient = GetComponentInParent<RecipientBehaviour>();
        m_recipient.objAddedEvent.AddListener(PlaySwallowAnimation);
        m_recipient.filledEvent.AddListener(PlaySuccessAnimation);
        m_recipient.wrongObjAdded.AddListener(PlayFailAnimation);
    }

    private void PlaySwallowAnimation()
    {
        m_animator.SetTrigger("Swallow");
    }

    private void PlaySuccessAnimation()
    {
        m_animator.SetTrigger("Success");
        GetComponent<Outline>().enabled = false;
    }

    private void PlayFailAnimation(InteractionController interactor)
    {
        m_animator.SetTrigger("Fail");
        objToThrow = GrabbableObjectInteractions.ReceiveItem(interactor);
        objToThrow.SetActive(false);
    }

    public void ThrowObj()
    {
        objToThrow.SetActive(true);
        StartCoroutine(GrabbableObjectInteractions.ThrownThenMove(objToThrow, stonePoint, Vector3.zero , throwForce, 1.2f));
    }
}
