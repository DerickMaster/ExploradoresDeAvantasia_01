using UnityEngine;

public class ZaHandoBehaviour : MonoBehaviour
{
    Animator m_Animator;
    [SerializeField] int initialValue;
    [SerializeField] bool selfActivated = true;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        if(selfActivated) SetHandValue(initialValue);
    }

    [ContextMenu("Test value change")]
    public void TestValueChange()
    {
        SetHandValue(initialValue);
    }

    public void SetHandValue(int value)
    {
        if (value > 5) value = 5;
        else if (value < 0) value = 0;

        m_Animator.SetFloat("Value", value);
    }

    public void PlayYesNoAnimation(bool yes)
    {
        if (yes) m_Animator.SetTrigger("Yes");
        else m_Animator.SetTrigger("No");
    }
}