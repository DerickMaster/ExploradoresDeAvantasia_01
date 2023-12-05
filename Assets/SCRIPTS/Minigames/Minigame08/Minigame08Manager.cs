using UnityEngine;
using UnityEngine.Events;

public class Minigame08Manager : MonoBehaviour, IMakeMistakes
{
    /*
     Lógica do minigame:
    - Manager aleatoriza o caminho
    - Sala aleatoriza qual bloco de gelo tem a palavra correta
    - Chegar no final do caminho é um sucesso
    - Caso jogador escolha uma palavra errada, gelo explode dando dano
    - Jogador não pode entrar em sala errada, ele se deparará com uma blockedzone e congelará.
     */

    [SerializeField] Minigame08RoomBehaviour _firstAreaRoom;
    [SerializeField] Minigame08RoomTrigger _firstAreaTrigger;
    [SerializeField] Minigame08RoomBehaviour[] _secondAreaRooms, _thirdAreaRooms;
    [SerializeField] Minigame08RoomTrigger[] _secondAreaTriggers, _thirdAreaTriggers;
    string _secondAreaIndex, _thirdAreaIndex;
    Minigame08CorridorTrigger[] _corridors;

    public string[] _words;
    [HideInInspector] public UnityEvent<string, MistakeData> mistakeEvent;

    private void Start()
    {
        _firstAreaTrigger.refRoomEntered.AddListener(RandomizeRoom);

        _corridors = GetComponentsInChildren<Minigame08CorridorTrigger>();
        foreach(var corridor in _corridors)
        {
            corridor.refCorridorEntered.AddListener(OpenRoom);
        }

        foreach(var area in _secondAreaTriggers)
        {
            area.refRoomEntered.AddListener(RandomizeRoom);
        }

        foreach (var area in _thirdAreaTriggers)
        {
            area.refRoomEntered.AddListener(RandomizeRoom);
        }

        foreach (var letterCube in FindObjectsOfType<Minigame08RoomBehaviour>())
        {
            letterCube.wrongCubeEvent.AddListener(CubeHit);
        } 

    }

    private void CubeHit(string rightAsnwer, string wrongAnswer)
    {
        mistakeEvent.Invoke("Labirinto de letras", new MistakeData { rightAnswer = rightAsnwer, mistakeMade = wrongAnswer });
    }

    void RandomizeRoom(Minigame08RoomBehaviour room)
    {
        int wordIndex = Random.Range(0, _words.Length);
        string newWord = _words[wordIndex];
        //_wordCinematics[wordIndex].Play();
        room.SetWord(newWord, wordIndex);
        room.RandomizeBlocks();
    }

    [SerializeField] GameObject[] _secondAreaRulers;
    [SerializeField] GameObject[] _secondAreaWrongRulers;
    void OpenRoom(int corridorIndex)
    {
        int doorToOpen = Random.Range(0, _secondAreaRooms.Length);
        if(corridorIndex == 1)
        {
            GenerateSecondAreaRoom(doorToOpen);
        }
        else
        {
            GenerateThirdAreaRoom(_secondAreaIndex);
        }
    }

    [SerializeField] ObjectDisabler[] _firstCorridorFlowersDisabler;
    void GenerateSecondAreaRoom(int index)
    {
        _secondAreaRooms[index].SwitchDoorState(true);
        _secondAreaRulers[index].SetActive(true);
        _secondAreaWrongRulers[index].SetActive(false);
        _firstCorridorFlowersDisabler[index].DisableObjects();
        _secondAreaIndex = (index + 1).ToString();
    }

    [SerializeField] GameObject[] _room01Rulers, _room02Rulers, _room03Rulers;
    [SerializeField] GameObject[] _room01WrongRulers, _room02WrongRulers, _room03WrongRulers;
    [SerializeField] ObjectDisabler[] _room01FlowersDisabler, _room02FlowersDisabler, _room03FloowersDisabler;
    [SerializeField] GameObject[] _zone02Vertical;
    [SerializeField] ObjectDisabler[] _zone02VerticalPlantsDisabler;
    void GenerateThirdAreaRoom(string secondAreaIndex)
    {
        int thirdIndex = 0;
        if (secondAreaIndex == "1")
        {
            thirdIndex = Random.Range(0,2);
            _room01Rulers[thirdIndex].SetActive(true);
            _room01WrongRulers[thirdIndex].SetActive(false);
            _room01FlowersDisabler[thirdIndex].DisableObjects();
        }else if(secondAreaIndex == "2")
        {
            thirdIndex = Random.Range(0, 3);
            _room02Rulers[thirdIndex].SetActive(true);
            _room02WrongRulers[thirdIndex].SetActive(false);
            _room02FlowersDisabler[thirdIndex].DisableObjects();
        }
        else
        {
            thirdIndex = Random.Range(0, 2);
            _room03Rulers[thirdIndex].SetActive(true);
            _room03WrongRulers[thirdIndex].SetActive(false);
            _room03FloowersDisabler[thirdIndex].DisableObjects();
            thirdIndex += 1;
        }
        _zone02Vertical[thirdIndex].SetActive(false);
        _zone02VerticalPlantsDisabler[thirdIndex].DisableObjects();
        _thirdAreaRooms[thirdIndex].SwitchDoorState(true);
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return mistakeEvent;
    }
}
