using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class av1minigamemanager : MonoBehaviour
{
    public static av1minigamemanager _instance = null;

    public GameObject _player;
    public CharacterController _ccReference;

    public List<GameObject> _regions;
    public GameObject _lastRegion;

    public int _newRegion;

    public int _newRegionCount = 0;

    public HealthManager _playerHpEvent;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _player.GetComponent<HealthManager>().hpModified.AddListener(CheckHealth);
        _ccReference = _player.GetComponent<CharacterController>();
    }

    public int _forwardOffset = 1;
    public void SendToNextRegion()
    {
        if (_newRegionCount == 5)
        {
            _newRegion = 999;
        }

        _ccReference.enabled = false;

        if(_newRegion == 999)
        {
            _player.transform.position = _lastRegion.transform.position + _lastRegion.transform.forward * _forwardOffset;
            _lastRegion.GetComponent<Animator>().Play("Entering");
        }
        else
        {
            _newRegionCount++;
            _newRegion = Random.Range(0, _regions.Count);

            _player.transform.position = _regions[_newRegion].transform.position + _regions[_newRegion].transform.forward * _forwardOffset;
            _regions[_newRegion].GetComponent<Animator>().Play("Entering");

            _regions.Remove(_regions[_newRegion]);
        }
        
        _ccReference.enabled = true;
    }

    public Text _canvasText;
    public string _defeatText;
    public GameObject _endgameCanvas;
    void CheckHealth(int health)
    {
        if(health == 0)
        {
            _canvasText.text = _defeatText;
            _endgameCanvas.SetActive(true);
        }
    }
}
