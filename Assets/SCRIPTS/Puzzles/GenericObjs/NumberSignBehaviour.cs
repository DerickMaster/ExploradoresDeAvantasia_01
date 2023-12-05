using UnityEngine;

public class NumberSignBehaviour : MonoBehaviour
{
    private int curNumber = -1;
    private Animator m_animator;
    private GameObject[] numbers;

    [SerializeField] private int newNumber = 0;
    [SerializeField] private GameObject numbersParent;
    [SerializeField] bool selfActivate = true;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        Transform[] temp = numbersParent.GetComponentsInChildren<Transform>();
        numbers = new GameObject[temp.Length - 1];
        for (int i = 1; i < temp.Length; i++)
        {
            numbers[i - 1] = temp[i].gameObject;
            numbers[i - 1].SetActive(false);
        }
        if (selfActivate) m_animator.SetBool("Active", true);
    }

    [ContextMenu("Test Change")]
    public void TestChange() 
    {
        TriggerChange(newNumber);
    }

    public void ActivateSign(int newNumber)
    {
        m_animator.SetBool("Active", true);
        TriggerChange(newNumber);
    }

    public void TriggerChange(int newNumber)
    {
        this.newNumber = newNumber;
        m_animator.SetTrigger("Change");
    }

    public void SwitchNumber()
    {
        if (curNumber == newNumber) return;

        if(curNumber >= 0)numbers[curNumber].SetActive(false);

        numbers[newNumber].SetActive(true);
        curNumber = newNumber;
    }
}
