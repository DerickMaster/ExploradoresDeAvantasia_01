using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorcupimPuzzleManager : MonoBehaviour
{
    private PorcupimBehaviour[] porcupins;
    StopPoint[] stops;
    private FarmyardBehaviour[] _farmyard;

    public InteractableObject _myObject;

    private void Start()
    {
        porcupins = GetComponentsInChildren<PorcupimBehaviour>();
        foreach(PorcupimBehaviour porcupim in porcupins)
        {
            porcupim.FinishedEating.AddListener(ChooseNewTarget);
        }
        _farmyard = GetComponentsInChildren<FarmyardBehaviour>();
        foreach(FarmyardBehaviour farmyard in _farmyard)
        {
            farmyard.filledYard.AddListener(CheckIfEnded);
        }
        stops = GetComponentsInChildren<StopPoint>();
    }

    private void ChooseNewTarget(PorcupimBehaviour porcupim)
    {
        int rngTarget = Random.Range(0, stops.Length);
        while (stops[rngTarget].occupied)
        {
            rngTarget = Random.Range(0, stops.Length);
        }
        porcupim.target = stops[rngTarget].gameObject;
        stops[rngTarget].occupied = true;
    }

    int _yardCount = 0;
    private void CheckIfEnded()
    {
        _yardCount++;
        if (_yardCount == _farmyard.Length)
        {
            GetComponent<ObjectSounds>().playObjectSound(0);
            _myObject.Interact(null);
        }
    }
}
