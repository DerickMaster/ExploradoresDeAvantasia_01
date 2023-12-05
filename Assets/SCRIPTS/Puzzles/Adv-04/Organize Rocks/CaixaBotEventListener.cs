using UnityEngine;
using UnityEngine.Events;

public class CaixaBotEventListener : MonoBehaviour
{
    [SerializeField] Material[] numMaterials;
    [SerializeField] Outline m_outline;
    [SerializeField] Animator m_animator;
    [SerializeField] bool activeFromStart = false;

    [HideInInspector] public UnityEvent finishedMoving;

    private void Start()
    {
        m_outline = GetComponentInChildren<Outline>();
        m_animator = GetComponent<Animator>();
        GetComponentInParent<RecipientBehaviour>().objAddedEvent.AddListener(PlaySwallow);

        if (activeFromStart) 
        {
            SetAnimationActive(true);
            SetMaterial(GetComponentInParent<RecipientBehaviour>().GetExpectedAmount());
        } 
    }

    public void SetAnimationActive(bool active)
    {
        m_animator.SetBool("Active", active);
    }

    private void PlaySwallow()
    {
        m_animator.SetTrigger("Swallow");
    }

    public void FinishedMoving()
    {
        finishedMoving.Invoke();
    }

    public void PlaySuccessOrFailAnimation(bool success)
    {
        if (success) m_animator.SetTrigger("Success");
        else m_animator.SetTrigger("Fail");
    }

    public void SetMaterial(int expectedCount)
    {
        m_outline.enabled = false;

        Material[] materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
        materials[1] = numMaterials[expectedCount - 1];
        GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

        m_outline.enabled = true;
    }
}
