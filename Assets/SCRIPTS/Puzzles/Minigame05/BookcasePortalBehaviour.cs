using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BookcasePortalBehaviour : InteractableObject
{
    [SerializeField] int[] m_ExpectedObjIds;
    [SerializeField] int baseId;
    [SerializeField] GameObject numbersParent;

    private Transform[] numbers;
    private Animator m_Animator;

    [HideInInspector] public UnityEvent<int> bookInserted;

    public int curAmount = 0;
    public int maxAmount = 0;
    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        numbers = numbersParent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < numbers.Length; i++)
        {
            numbers[i].gameObject.SetActive(false);
        }
    }

    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject && m_ExpectedObjIds.Contains(interactor.heldObject.GetComponent<GrabbableObject>().objectID))
        {
            int value = interactor.heldObject.GetComponent<GrabbableObject>().objectID - baseId;
            if (curAmount + value <= maxAmount)
            {
                m_Animator.SetTrigger("Activate");
                GrabbableObjectInteractions.SwallowItem(interactor);
                SwitchBlockedState();
                bookInserted.Invoke(value);
            }
            else
            {
                CanvasBehaviour.Instance.SetActiveTempText("Nao ha espaco para esses livros", 2f);
            }
        }
    }

    public void SetVisibleNumber(int amount)
    {
        numbers[amount - 1].gameObject.SetActive(true);
    }

    public bool CheckExpectedId(int id)
    {
        return m_ExpectedObjIds.Contains(id);
    }
}
