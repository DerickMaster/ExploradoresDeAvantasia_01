using UnityEngine.Events;
using UnityEngine;

public interface IEndGame
{
    public UnityEvent<string> GetEndGameEvent();
}