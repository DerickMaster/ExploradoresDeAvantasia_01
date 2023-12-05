using UnityEngine;

public class av1minigame2manager : MonoBehaviour
{
    public static av1minigame2manager _instance = null;
    public FruitBombAutoSpawner bombSpawner;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    

    public ZoneManager[] _zoneManagers;

    public GameObject _player;
    public CharacterController _playerCC;
    public HealthManager _playerHpEvent;
    public void Start()
    {
        _player = FindObjectOfType<CharacterManager>().GetCurrentCharacter();
        _playerCC = _player.GetComponent<CharacterController>();
        _playerHpEvent = _player.GetComponent<HealthManager>();
        StartGame();

        //UIDifficultyDetector difficultyDetector = FindObjectOfType<UIDifficultyDetector>();
        //difficultyDetector.easyChosenEvent.AddListener(SetAsEasy);
        //difficultyDetector.hardChosenEvent.AddListener(SetAsHard);
    }

    public GameObject[] _arrows;
    public bool hard = false;
    [SerializeField] private UnityEngine.Playables.PlayableDirector cutscene;
    public void StartGame()
    {
        cutscene.Play();
        FadeUICinematic.Instance.FadeOut();
        if (hard)
        {
            foreach (GameObject _arrow in _arrows)
            {
                _arrow.SetActive(false);
            }
        }
    }
    
    public int _currentRegion = 0;
    public void SendToNextRegion()
    {
        if (_currentRegion == 3)
            CanvasBehaviour.Instance.ActivateEndgameScreen();
        else
        {
            if (bombSpawner)
            {
                bombSpawner.Despawn();
            }
            _zoneManagers[_currentRegion].MovePlayer(_player, _playerCC);
        }
            
    }
}
