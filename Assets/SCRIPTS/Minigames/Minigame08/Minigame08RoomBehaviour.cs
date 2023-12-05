using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class Minigame08RoomBehaviour : MonoBehaviour
{
    [SerializeField] DoorBehaviour _myEntrance, _myExit;

    [SerializeField] private Mesh[] lettersMesh;
    private Dictionary<string, Mesh> meshDict;

    readonly string letters = "ABCDEFGHIJKLMOPQRSTUVWXYZ";

    [SerializeField] Minigame08LetterCube[] _myCubes;

    [SerializeField] string _myWord;
    [SerializeField] List<string> _correctVowels;
    int _currentVowelIndex = 0;
    [HideInInspector] public UnityEvent<string,string> wrongCubeEvent;

    private void Start()
    {
        meshDict = new Dictionary<string, Mesh>(lettersMesh.Length);
        foreach (var item in lettersMesh)
        {
            meshDict.Add(item.name.Substring(item.name.Length - 1, 1), item); //Fazendo um dicionário que usa o ultimo caracter do objeto, anexando à um mesh
        }
        _myCubes = GetComponentsInChildren<Minigame08LetterCube>();
        _myCinematics = GetComponentsInChildren<PlayableDirector>();

        foreach (var cinematic in _myCinematics)
        {
            cinematic.gameObject.SetActive(false);
        }

        foreach(var item in _myCubes)
        {
            item.refUnitKilled.AddListener(LetterHit);
        }
    }

    [SerializeField] PlayableDirector[] _myCinematics;
    int _cinIndex;
    public void SetWord(string newWord, int cinematicIndex)
    {
        _cinIndex = cinematicIndex;
        _myCinematics[cinematicIndex].gameObject.SetActive(true);
        _myCinematics[cinematicIndex].Play();
        _myWord = newWord;
        RemoveVowels(_myWord);
    }

    void RemoveVowels(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (IsAVowel(word[i])) _correctVowels.Add(word[i].ToString());
        }
    }

    bool IsAVowel(char _char)
    {
        if (_char == 'A' || _char == 'E' || _char == 'I' || _char == 'O' || _char == 'U') return true;
        else return false;
    }

    public void RandomizeBlocks()
    {
        int cubeIndex = Random.Range(0, _myCubes.Length);
        int randomLetter = Random.Range(0, meshDict.Count);
        for (int i = 0; i < _myCubes.Length; i++)
        {
            if (i == cubeIndex)
            {
                _myCubes[i].UpdateChar(_correctVowels[_currentVowelIndex], meshDict[_correctVowels[_currentVowelIndex]], true);
            }
            else
            {
                randomLetter = Random.Range(0, meshDict.Count-1);
                while (letters[randomLetter].ToString() == _correctVowels[_currentVowelIndex])
                {
                    randomLetter = Random.Range(0, meshDict.Count);
                }
                _myCubes[i].UpdateChar(letters[randomLetter].ToString(), meshDict[letters[randomLetter].ToString()], false) ;
            }
        }
    }

    void LetterHit(Minigame08LetterCube cube)
    {
        //StartCoroutine(ReEnableCube());
        if (cube._correctAnswer)
        {
            ReenableVowel(_cinIndex, _currentVowelIndex);
            _currentVowelIndex++;
        }
        else
        {
            wrongCubeEvent.Invoke(cube.myChar, _correctVowels[_currentVowelIndex]);
        }

        DisableCubes();
        StartCoroutine(RandomizeThenActivate());

        //if(_currentVowelIndex < _correctVowels.Count) RandomizeBlocks();

        if (_currentVowelIndex == _correctVowels.Count)
        {
            _myExit.OpenDoor();
            DisableCubes();
        }
    }

    private void DisableCubes()
    {
        foreach (var cube in _myCubes)
        {
            cube.DisableCube();
        }
    }

    private IEnumerator RandomizeThenActivate()
    {
        yield return new WaitForSeconds(2f);

        RandomizeBlocks();
        ReEnableCubes();
    }

    [SerializeField] GameObject[] _firstWordVowels, _secondWordVowels, _thirdWordVowels;
    void ReenableVowel(int cinIndex, int vowelIndex)
    {
        if(cinIndex == 0)
        {
            _firstWordVowels[vowelIndex].SetActive(true);
        }else if(cinIndex == 1)
        {
            _secondWordVowels[vowelIndex].SetActive(true);
        }
        else
        {
            _thirdWordVowels[vowelIndex].SetActive(true);
        }
    }

 
    private void ReEnableCubes()
    {
        foreach (var cube in _myCubes)
        {
            cube.ActivateCrystal();
        }
    }

    public void SwitchDoorState(bool opening)
    {
        if (opening) _myEntrance.OpenDoor();
        else _myEntrance.CloseDoor();
    }

    public void CloseEntrance()
    {
        _myEntrance.CloseDoor();
    }
}
