using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainThrashCollectorBehaviour : MonoBehaviour, IMakeMistakes, IEndGame
{
    public GameObject CollectorsParent;
    private ThrashCollectorBehaviour[] m_collectors;
    [SerializeField] private int curHealth;
    [SerializeField] private int maxHealth;

    private Animator m_animator;
    public UnityEvent GameWon;
    public UnityEvent GameLost;

    public int curThrashes;
    public int expectedThrashes;

    [HideInInspector] public UnityEvent<string, MistakeData> _wrongRobot;
    public string _puzzleName;
    public MistakeData _mistakeDescription;

    private void Awake()
    {
        _endGameEvent = new UnityEvent<string>();
    }

    private void Start()
    {

        m_animator = GetComponent<Animator>();
        curHealth = maxHealth;

        m_collectors = CollectorsParent.GetComponentsInChildren<ThrashCollectorBehaviour>();
        foreach(ThrashCollectorBehaviour collector in m_collectors)
        {
            collector.Initialize(transform.position);
            collector.reachedMain.AddListener(GetThrashId);
        }
    }

    private void GetThrashId(ThrashCollectorBehaviour collector)
    {
        m_animator.SetBool("Swallow", true);
        curThrashes++;
        if (curThrashes >= expectedThrashes) GameWon.Invoke();

        if (!collector.CheckReceivedItem()) 
        {
            m_animator.SetBool("Good", false);
            _mistakeDescription.rightAnswer = GetColorById(collector.m_KeyObjectID);
            _mistakeDescription.mistakeMade = GetColorById(collector._receivedItemID);
            _wrongRobot.Invoke(_puzzleName, _mistakeDescription);
            curHealth -= 2;
            if(curHealth <= 0)
            {
                Die();
            }
        }else
        {
            m_animator.SetBool("Good", true);
        }
    }

    public void ResetBools()
    {
        m_animator.SetBool("Swallow", false);
        ShowLine();
    }

    [SerializeField] Sprite _recicleBotSprite;
    [SerializeField] FMODUnity.EventReference[] _recicleDubs;
    [SerializeField, TextArea] string _successText;
    [SerializeField, TextArea] string _errorText;
    public void ShowLine()
    {
        if (m_animator.GetBool("Good")) CanvasBehaviour.Instance.SetActiveTempText(_successText, 2f, _recicleBotSprite, _recicleDubs[0]);
        else CanvasBehaviour.Instance.SetActiveTempText(_errorText, 2f, _recicleBotSprite, _recicleDubs[1]);
    }

    private void Die()
    {
        GameLost.Invoke();
        _endGameEvent.Invoke("WrongType");
    }

    public UnityEvent<string,MistakeData> GetMistakeEvent()
    {
        return _wrongRobot;
    }

    UnityEvent<string> _endGameEvent;
    public UnityEvent<string> GetEndGameEvent()
    {
        return _endGameEvent;
    }

    private string GetColorById(int id)
    {
        switch (id)
        {
            case 101:
                return "Amarelo";
            case 102:
                return "Azul";
            case 103:
                return "Verde";
            case 104:
                return "Vermelho";
            default:
                return "Erro";
        }
    }
}
