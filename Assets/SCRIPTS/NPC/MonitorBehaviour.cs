using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonitorBehaviour : InteractableObject
{
    public GameObject _zoneStart;
    public DialogueObject _myDialog;
    public GameObject _invWall;

    public MonitorWrongEntry[] _wrongDoors;

    public GameObject _players;

    CharacterManager _cManager;


    public override void Interact(InteractionController interactor)
    {
        if (_invWall.activeSelf)
        {
            _invWall.SetActive(false);
        }
        if(_myDialog != null)DialogueBoxManager.Instance.PlayDialogue(_myDialog, interactor);
    }

    private new void Start()
    {
        _cManager = FindObjectOfType<CharacterManager>();
        _wrongDoors = GetComponentsInChildren<MonitorWrongEntry>();

        if(_wrongDoors != null)
        {
            for (int i = 0; i < _wrongDoors.Length; i++)
            {
                _wrongDoors[i]._wrongEnter.AddListener(ResetPosition);
            }
        }
    }

    private void ResetPosition()
    {
        _cManager.GetCurrentCharacter().GetComponent<CharacterController>().enabled = false;
        _cManager.GetCurrentCharacter().transform.position = _zoneStart.transform.position;
        _cManager.GetCurrentCharacter().GetComponent<CharacterController>().enabled = true;
    }
}
