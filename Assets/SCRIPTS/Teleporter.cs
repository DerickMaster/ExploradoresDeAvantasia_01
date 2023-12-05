using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject _finalPosition;
    [SerializeField]
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Teleport();
    }

    public void Teleport()
    {
        _player.GetComponent<CharacterController>().enabled = false;
        _player.transform.position = _finalPosition.transform.position;
        _player.GetComponent<CharacterController>().enabled = true;
    }
}
