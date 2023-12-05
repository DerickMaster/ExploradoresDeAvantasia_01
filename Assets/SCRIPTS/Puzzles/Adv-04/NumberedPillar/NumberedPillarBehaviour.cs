using UnityEngine;

public class NumberedPillarBehaviour : MonoBehaviour
{
    private Animator m_Animator;
    [SerializeField] private int Value;
    [SerializeField] private OperationButtonBehaviour[] buttons;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();

        ModifyValue(0);
        foreach (OperationButtonBehaviour button in buttons)
        {
            button.pressedEvent.AddListener(ModifyValue);
        }
        m_Animator.SetInteger("Floor", Value);
    }

    private void ModifyValue(int amount)
    {
        Value += amount;
        if (Value <= 0)
        {
            Value = 0;
            buttons[0].SetBlocked(true);
        }
        else if (Value >= 4)
        {
            Value = 4;
            buttons[1].SetBlocked(true);
        }
        else
        {
            buttons[0].SetBlocked(false);
            buttons[1].SetBlocked(false);
        }

        m_Animator.SetInteger("Floor", Value);
    }
}
