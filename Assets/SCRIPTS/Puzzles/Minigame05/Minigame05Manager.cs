using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Minigame05Manager : MonoBehaviour, IMakeMistakes
{
    GnomioBehaviour _Gnomio;

    BookcaseBehaviour[] bookcases;
    [SerializeField] int minAmount;
    [SerializeField] int maxAmount;

    private int curBookId = -1;
    private ActivationBookBehaviour[] books;

    [SerializeField] PlayableDirector _cinStart;
    [HideInInspector] public UnityEvent<string, MistakeData> mistakeEvent;

    [SerializeField] FMODUnity.EventReference[] _sounds;

    private void Awake()
    {
        bookcases = GetComponentsInChildren<BookcaseBehaviour>();
        _Gnomio = GetComponentInChildren<GnomioBehaviour>();

        foreach (BookcaseBehaviour bookcase in bookcases)
        {
            bookcase.bookcaseSubmitted.AddListener(CheckBookcase);
            bookcase.SetExpectedAmount(Random.Range(minAmount, maxAmount+1));
        }

        int id = 0;
        books = GetComponentsInChildren<ActivationBookBehaviour>();
        foreach (ActivationBookBehaviour bookBehaviour in books)
        {
            bookBehaviour.objectID = id;
            bookBehaviour.bookInteractedEvent.AddListener(DeactivatePreviousPlatform);
            id++;
        }

        foreach (var item in GetComponentsInChildren<SpawnBookSlot>())
        {
            item.SpawnPrefab();
            item.bookPickedUpEvent.AddListener(SwitchPortalBlockedState);
        }
        targetAmount = bookcases.Length;
    }

    private void SwitchPortalBlockedState(bool grabbed, int id)
    {
        foreach (BookcaseBehaviour bookcase in bookcases)
        {
            if (bookcase.m_Portal.CheckExpectedId(id))
            {
                bookcase.m_Portal.SwitchBlockedState();
            }
        }
    }

    private void Start()
    {
        _cinStart.Play();

        foreach (BookcasePortalBehaviour portal in GetComponentsInChildren<BookcasePortalBehaviour>())
        {
            portal.SwitchBlockedState();
        }
    }

    private int curAmountSolved = 0;
    private int targetAmount = 0;
    [SerializeField] string _errorMessageMore, _errorMessageLess, _expectedMessage;
    private void CheckBookcase(BookcaseBehaviour bookcase)
    {
        if(bookcase.curAmount == bookcase.expectedAmount)
        {
            CanvasBehaviour.Instance.SetActiveTempText(_expectedMessage, 2f, _Gnomio.GetGnomioSprite(), _sounds[0]);
            bookcase.TurnOff();
            curAmountSolved++;
            _Gnomio.PlayCorrectAnimation();
            if (curAmountSolved >= targetAmount) WinGame();
        }
        else
        {
            _Gnomio.PlayWrongAnimation();
            if(bookcase.curAmount < bookcase.expectedAmount) CanvasBehaviour.Instance.SetActiveTempText(_errorMessageLess, 2f, _Gnomio.GetGnomioSprite(), _sounds[1]);
            else CanvasBehaviour.Instance.SetActiveTempText(_errorMessageMore, 2f, _Gnomio.GetGnomioSprite(), _sounds[2]);

            mistakeEvent.Invoke("Livros e estantes",new MistakeData { mistakeMade = bookcase.curAmount.ToString(), rightAnswer = bookcase.expectedAmount.ToString() } );
            bookcase.ResetBookcase();
        }
    }

    private void WinGame()
    {
        CanvasBehaviour.Instance.ActivateEndgameScreen();
    }

    private void DeactivatePreviousPlatform(ActivationBookBehaviour book, bool open, bool playerInteracted)
    {
        if (!playerInteracted) return;

        if (open)
        {
            if (curBookId >= 0 && curBookId != book.objectID)
            {
                books[curBookId].Interact(null);
            }
            curBookId = book.objectID;
        }
        else
        {
            curBookId = -1;
        }
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return mistakeEvent;
    }
}