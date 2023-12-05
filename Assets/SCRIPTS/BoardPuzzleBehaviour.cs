using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BoardPuzzleBehaviour : InteractableObject, IMakeMistakes
{
    public StarterAssets.StarterAssetsInputs playerInput;
    private bool active = false;
    private bool moving = false;

    public GameObject moveableObj;
    public bool solved;

    public float distance = 1.75f;
    public float speed = 1f;

    public Vector2 rightDirection;

    [SerializeField] private GameObject m_camera;
    [SerializeField] private GameObject main_camera;

    [SerializeField] Animator robotAnimator;
    [SerializeField] GameObject handSlot;
    [SerializeField] GameObject boardSlot;

    [SerializeField] private HandRobotBehaviour robotBehaviour;

    public GameObject dpad;

    [HideInInspector] public UnityEvent<string, MistakeData> _wrongDirection;
    public string _puzzleName;
    public MistakeData _mistakeDescription;

    private void OnValidate()
    {
        //robotBehaviour = GetComponentInChildren<HandRobotBehaviour>();
    }
    private new void Start()
    {
        main_camera = GameObject.FindGameObjectWithTag("MainCamera");
        robotBehaviour.Initialize(handSlot, moveableObj, boardSlot);
        robotBehaviour.activate.AddListener(ActivatePuzzle);
    }

    public override void Interact(InteractionController interactor)
    {
        active = robotAnimator.GetBool("Active");
        robotAnimator.SetBool("Woke", true);
        robotAnimator.SetBool("Active", true);
       
        playerInput = interactor.gameObject.GetComponentInParent<StarterAssets.StarterAssetsInputs>();
        interactor.SwitchActionMap("BoardControl");
        CanvasBehaviour.Instance.SetActiveButtonList(false);
        SwitchCamera(true);
        CanvasBehaviour.Instance.SetActiveDPad(true);
    }

    public void ActivatePuzzle()
    {
        active = true;
    }

    private void SwitchCamera(bool active)
    {
        main_camera.SetActive(!active);
        m_camera.SetActive(active);
    }

    private void Update()
    {
        if (active && !moving) 
        {
            DetectInput();
            Cancel();
        }
    }

    private void DetectInput()
    {
        if ( 0.9f < playerInput.moveDirection.sqrMagnitude && playerInput.moveDirection.sqrMagnitude < 1.1f)
        {
            DirectionInputted();
        }
    }

    private void DirectionInputted()
    {
        StartCoroutine(MoveHand(playerInput.moveDirection.normalized));
    }

    private void Cancel()
    {
        if (playerInput.cancel)
        {
            playerInput.cancel = false;
            ExitPuzzle();
        }
    }

    private void ExitPuzzle()
    {
        playerInput.gameObject.GetComponent<StarterAssets.ThirdPersonController>().enabled = true;

        active = false;
        robotAnimator.SetBool("Woke", active);

        InteractionController.Instance.SwitchActionMap();
        CanvasBehaviour.Instance.SetActiveButtonList(true);
        SwitchCamera(false);
        CanvasBehaviour.Instance.SetActiveDPad(false);
    }

    float timeElapsed;
    public float duration = 2f;

    private IEnumerator MoveHand(Vector2 direction, Vector2 startPosition = default, bool returnMove = false)
    {
        timeElapsed = 0f;
        moving = true;
        Vector2 position;

        while (timeElapsed < duration)
        {
            position = Vector2.Lerp(startPosition, direction, (timeElapsed / duration));
            robotAnimator.SetFloat("X-Pos", position.x);
            robotAnimator.SetFloat("Y-Pos", -position.y);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        moving = false;

        if (returnMove) yield break;

        if (CheckAnswer(direction)) PuzzleSolved();
        else
        {
            if(direction == Vector2.up)
            {
                _mistakeDescription.mistakeMade = "Moveu para o triângulo";
            }
            else if (direction == Vector2.right)
            {
                _mistakeDescription.mistakeMade = "Moveu para o quadrado";
            }
            else if (direction == Vector2.left)
            {
                _mistakeDescription.mistakeMade = "Moveu para o fractal";
            }
            else if (direction == Vector2.down)
            {
                _mistakeDescription.mistakeMade = "Moveu para o circulo";
            }
            _wrongDirection.Invoke(_puzzleName,_mistakeDescription);
            StartCoroutine(MoveHand(new Vector2(0f, 0f), direction  , true));
        }
    }

    private bool CheckAnswer(Vector2 inputtedDirection)
    {
        Debug.Log(inputtedDirection.normalized.ToString("F8"));
        if (inputtedDirection.normalized == rightDirection) return true;
        else return false;
    }

    private void PuzzleSolved()
    {
        solved = true;
        robotAnimator.SetBool("Drop", true);
        robotAnimator.SetBool("Active", false);

        robotBehaviour.DropObject();

        DeactivateInteractable();
        ExitPuzzle();
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return _wrongDirection;
    }
}