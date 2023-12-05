using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PrintPuzzleManager : MonoBehaviour
{
    private bool initialized = false;
    private Animator m_animator;
    private PrintSquare[] Grid;
    private GameObject curSquare;
    [SerializeField] private GameObject Selector;

    [TextArea]
    public string Phrase;
    private string[] words;

    public int curWord = 0;

    private bool moving;
    public bool finished = false;

    public StarterAssets.StarterAssetsInputs playerInput;
    public UnityEvent closePuzzleEvent;

    void ResetMinigame()
    {
        words = Phrase.Split(' ');

        for (int i = 0; i < Grid.Length; i++)
        {
            if(i < words.Length)
                Grid[i].SetWord(words[i]);
            Grid[i].SetActiveText(false);
        }
        curSquare = Grid[4].gameObject;
        curWord = 0;
        curPos = Vector2.zero;
        m_animator.SetFloat("X", curPos.x);
        m_animator.SetFloat("Y", curPos.y);
    }

    public void Initialize(InteractionController interactor)
    {
        if (!initialized)
        {
            m_animator = GetComponentInChildren<Animator>();
            GetComponentInChildren<PrinterAnimationReceiver>().stampEvent.AddListener(SetWordOnSlot);
            Grid = GetComponentsInChildren<PrintSquare>();
            rotationAngle = Camera.main.transform.eulerAngles.y;
            initialized = true;
        }

        playerInput = interactor.GetComponentInParent<StarterAssets.StarterAssetsInputs>();
        m_animator.SetBool("inPuzzle", true);

        CanvasBehaviour.Instance.SetActiveButtonList(false);
        CanvasBehaviour.Instance.SetActiveDPad(true);
        
        ResetMinigame();
        curSquare.GetComponent<PrintSquare>().SetActiveOutline(true);
    }

    private bool pressed;
    private void Update()
    {
        if (!moving && !pressed)
        {
            DetectInput();
        }
        else if(playerInput.moveDirection.magnitude == 0f) pressed = false;

        if (playerInput.submit)
        {
            playerInput.submit = false;
            SubmitWord();
        }
    }

    private void DetectInput()
    {
        if (0.9f < playerInput.moveDirection.magnitude && playerInput.moveDirection.magnitude < 1.1f)
        {
            pressed = true;
            DirectionInputted();
        }
    }

    [SerializeField] LayerMask mask;
    private Vector2 curPos = new Vector2();
    private float rotationAngle;
    private void DirectionInputted()
    {
        Vector3 direction = Quaternion.Euler(0, rotationAngle, 0) * new Vector3(playerInput.moveDirection.x, 0, playerInput.moveDirection.y);

        RaycastHit hit;
        if (Physics.Raycast(curSquare.transform.position, direction, out hit, 3f, mask))
        {
            curSquare.GetComponent<PrintSquare>().SetActiveOutline(false);
            curSquare = hit.transform.gameObject;

            StartCoroutine(MovePress(curPos - new Vector2(direction.x, direction.z)));
        }
    }

    [SerializeField] float moveTime; 
    private IEnumerator MovePress(Vector2 targetVector)
    {
        moving = true;
        Vector2 initialPos = curPos;
        float curTime = 0f;

        while (curTime < moveTime)
        {
            curPos = Vector2.Lerp(initialPos, targetVector, curTime / moveTime);
            m_animator.SetFloat("X", curPos.x);
            m_animator.SetFloat("Y", curPos.y);
            curTime += Time.deltaTime;

            yield return null;
        }
        curSquare.GetComponent<PrintSquare>().SetActiveOutline(true);
        moving = false;
    }

    [ContextMenu("Submit word")]
    private void SubmitWord()
    {
        if (moving) return;

        if (curSquare == Grid[curWord].gameObject)
        {
            m_animator.SetTrigger("Stamp");
            moving = true;
        }
        else
            CanvasBehaviour.Instance.SetActiveTempText("Local errado", 3f);
    }

    private void SetWordOnSlot()
    {
        curSquare.GetComponent<PrintSquare>().SetActiveText(true);
        curWord++;
        moving = false;
        if (curWord >= words.Length) FinishPuzzle();
    }

    private void FinishPuzzle()
    {
        curSquare.GetComponent<PrintSquare>().SetActiveOutline(false);
        m_animator.SetBool("inPuzzle", false);
        finished = true;
        closePuzzleEvent.Invoke();
    }

    [ContextMenu("Select Square")]
    private void SelectSquare()
    {
        curSquare = Grid[curWord].gameObject;
        Selector.transform.position = curSquare.transform.position;
    }

    public void DeactivateSelector()
    {
        Selector.SetActive(false);
    }
}
