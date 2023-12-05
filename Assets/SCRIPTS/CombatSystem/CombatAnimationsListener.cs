using UnityEngine;
using UnityEngine.Events;

public class CombatAnimationsListener : MonoBehaviour
{
    [HideInInspector] public UnityEvent startCheckingArea;
    [HideInInspector] public UnityEvent stopCheckingArea;

    public void StartCheckingArea()
    {
        startCheckingArea.Invoke();
    }

    public void StopCheckingArea()
    {
        stopCheckingArea.Invoke();
    }
}