using UnityEngine.Events;
using UnityEngine;

public interface IEndGame
{
    public UnityEvent<bool, float> GetEndGameEvent();
}