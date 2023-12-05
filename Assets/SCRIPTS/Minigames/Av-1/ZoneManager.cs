using UnityEngine;
using UnityEngine.Events;

public class ZoneManager : MonoBehaviour, IMakeMistakes
{
    //Ordem dos portais: Baixo, Cima, Direita, Esquerda
    public GameObject _zoneCenter;
    public Bomb_Door[] _myBombDoors;
    public DoorBehaviour[] _myDoors;
    public BlockedZoneBehaviour[] _myBlockedZones;

    [SerializeField] int _repeatedTimes = 0;
    [HideInInspector] public UnityEvent<string, MistakeData> mistakeMade;

    private void Start()
    {
        for(int i = 0; i < _myBombDoors.Length; i++)
        {
            _myBombDoors[i].id = i;
            _myBombDoors[i]._doorOpenedEvent.AddListener(IncreaseCount);
            _myBlockedZones[i].zoneID = i;
            _myBlockedZones[i].touched.AddListener(ZoneTriggered);
        }
    }

    public Animator _portalAnimator;
    public void MovePlayer(GameObject player, CharacterController cc)
    {
        _repeatedTimes++;

        RandomizePortal();

        cc.enabled = false;
        player.transform.position = _zoneCenter.transform.position;
        cc.enabled = true;

        _portalAnimator.Play("Spitting");

        if (_repeatedTimes == 2)
            av1minigame2manager._instance._currentRegion++;
    }

    public int _myRandom = 0;
    void RandomizePortal()
    {
        _myRandom = Random.Range(0, _myBombDoors.Length);
        AdjustArrow(_myRandom);

        for (int i = 0; i < _myBombDoors.Length; i++)
        {
            _myDoors[i].CloseDoor();
            _myBombDoors[i].CloseDoor();
            _myBombDoors[i]._bombCount = 0;
            if (i != _myRandom)
            {
                _myDoors[i].transform.parent.gameObject.SetActive(false);
                _myBombDoors[i].transform.parent.gameObject.SetActive(true);
                _myBlockedZones[i].allowPassage = false;
            }
            else
            {
                _myDoors[i].transform.parent.gameObject.SetActive(true);
                _myBombDoors[i].transform.parent.gameObject.SetActive(false);
                _myBlockedZones[i].allowPassage = true;
            }
        }
    }

    public Animator _myArrows;
    void AdjustArrow(int direction)
    {
        _myArrows.SetBool("South", false);
        _myArrows.SetBool("North", false);
        _myArrows.SetBool("Right", false);
        _myArrows.SetBool("Left", false);

        if (direction == 0)
        {
            _myArrows.SetBool("South", true);
        }
        else if (direction == 1)
        {
            _myArrows.SetBool("North", true);
        }
        else if (direction == 2)
        {
            _myArrows.SetBool("Right", true);
        }
        else if (direction == 3)
        {
            _myArrows.SetBool("Left", true);
        }
    }

    private string GetDirectionByName(int direction)
    {
        switch (direction)
        {
            case 0:
                return "Atras";
            case 1:
                return "Frente";
            case 2:
                return "Direita";
            case 3:
                return "Esquerda";
            default:
                return "Erro";
        }
    }

    void IncreaseCount(Bomb_Door bombDoorTriggered)
    {
        MistakeData mistakeData = new MistakeData { rightAnswer = GetDirectionByName(_myRandom),
                                                    mistakeMade = GetDirectionByName(bombDoorTriggered.id) 
                                                    };
        mistakeMade.Invoke("PortaAberta", mistakeData);

        foreach(Bomb_Door door in _myBombDoors)
        {
            door._bombCount++;
        }
    }

    private void ZoneTriggered(BlockedZoneBehaviour blockedZoneTriggered)
    {
        MistakeData mistakeData = new MistakeData
        {
            rightAnswer = GetDirectionByName(_myRandom),
            mistakeMade = GetDirectionByName(blockedZoneTriggered.zoneID)
        };
        mistakeMade.Invoke("Andou pro lado errado", mistakeData);
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return mistakeMade;
    }
}
