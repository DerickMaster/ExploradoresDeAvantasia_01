using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatformBehaviour : MonoBehaviour
{
    enum PlatformState
    {
        Waiting,
        Moving
    }

    [SerializeField] GameObject platform;
    public Vector3 movementMod;
    public float speed;
    PlatformState state;
    float waitTime;

    private void Start()
    {
        state = PlatformState.Waiting;
    }

    private void Update()
    {
        waitTime += Time.deltaTime;
        switch (state)
        {
            case PlatformState.Moving:
                MovingBehaviour();
                break;
            case PlatformState.Waiting:
                WaitingBehaviour();
                break;
        }
    }

    private void MovingBehaviour()
    {
        platform.transform.position = platform.transform.position + (movementMod * (speed * Time.deltaTime));
        if (waitTime >= 5f)
        {
            waitTime = 0f;
            state = PlatformState.Waiting;
        }
    }

    private void WaitingBehaviour()
    {
        if (waitTime >= 3f)
        {
            waitTime = 0f;
            state = PlatformState.Moving;
            movementMod *= -1f;
        }
    }
}
