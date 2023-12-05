using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BookcaseBehaviour : InteractableObject
{
    [SerializeField] public int maxBookshelfAmount;
    [SerializeField] public int expectedAmount;
    public int curAmount;
    public BookcasePortalBehaviour m_Portal;
    private Coroutine curCoroutine;
    [SerializeField] GameObject slotsParent;
    [SerializeField] Animator[] bookshelfs;

    [SerializeField] GameObject bookshelfBasePrefab;
    [SerializeField] GameObject bookshelfIncreasePrefab;
    [SerializeField] Material m_BookcaseMat;

    [HideInInspector] public UnityEvent<BookcaseBehaviour> bookcaseSubmitted;

    private new void Start()
    {
        base.Start();

        bookshelfs = new Animator[maxBookshelfAmount];
        for (int i = 0; i < maxBookshelfAmount; i++)
        {
            GameObject prefab;
            if (i == 0) prefab = bookshelfBasePrefab;
            else prefab = bookshelfIncreasePrefab;

            bookshelfs[i] = Instantiate(prefab, slotsParent.transform).GetComponentInChildren<Animator>();
            bookshelfs[i].transform.parent.localPosition = new Vector3(0, 1.4f * i, 0);

            Material[] tempMats = bookshelfs[i].GetComponentInChildren<Renderer>().materials;
            tempMats[0] = m_BookcaseMat;
            bookshelfs[i].GetComponentInChildren<Renderer>().materials = tempMats;

            bookshelfs[i].gameObject.SetActive(false);
        }
        m_Portal = GetComponentInChildren<BookcasePortalBehaviour>();
        m_Portal.bookInserted.AddListener(IncreaseAmount);
        m_Portal.maxAmount = maxBookshelfAmount;
        m_Portal.SetVisibleNumber(expectedAmount);
    }

    public override void Interact(InteractionController interactor)
    {
        SubmitAmount();
    }

    [ContextMenu("ResetBookcase")]
    public void ResetBookcase()
    {
        curCoroutine = StartCoroutine(LowerBookshelfs());
        curAmount = 0;
        m_Portal.curAmount = curAmount;
    }

    IEnumerator LowerBookshelfs()
    {
        int amount = curAmount-1;
        for (int i = amount; i >= 0; i--)
        {
            if (i < bookshelfs.Length-1) bookshelfs[i + 1].gameObject.SetActive(false);
            bookshelfs[i].SetBool("Active", false);
            yield return new WaitForSeconds(delayTime);
        }
        bookshelfs[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        GetComponentInChildren<LauraGear>().RactivateGear();

        curCoroutine = null;
    }

    public void TurnOff()
    {
        m_Portal.gameObject.SetActive(false);
        DeactivateInteractable();
    }

    public void IncreaseAmount(int increaseValue)
    {
        curCoroutine = StartCoroutine(RaiseBookshelfs(increaseValue));
    }

    [SerializeField] float delayTime = 0f;
    IEnumerator RaiseBookshelfs(int increaseValue)
    {
        int prevAmount = curAmount;
        curAmount += increaseValue;
        for (int i = 0; i < increaseValue; i++)
        {
            bookshelfs[prevAmount + i].gameObject.SetActive(true);
            bookshelfs[prevAmount + i].SetBool("Active", true);
            yield return new WaitForSeconds(delayTime);
        }
        m_Portal.curAmount = curAmount;
        curCoroutine = null;
    }

    public void SubmitAmount()
    {
        StartCoroutine(SubmitAmountCoroutine());
    }

    private IEnumerator SubmitAmountCoroutine()
    {
        while(curCoroutine != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        bookcaseSubmitted.Invoke(this);
    }

    public void SetExpectedAmount(int amount)
    {
        expectedAmount = amount;
    }
}