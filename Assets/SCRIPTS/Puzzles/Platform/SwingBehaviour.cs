using UnityEngine;
using System.Collections;

public class SwingBehaviour : MonoBehaviour
{
    PlatformBehaviour m_Platform;
    Animator _animator;
    float moveTime = 0f;
    float lastValue = 0f;

    bool movePlayer = false;
    bool moving = false;

    [SerializeField] float finalValue;
    [SerializeField] float totalTime;
    [SerializeField] float moveSpeed;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        m_Platform = GetComponentInChildren<PlatformBehaviour>();
        m_Platform.playerEnteredEvent.AddListener(ActivateMovePlayer);
        lastValue = -finalValue;
        moving = true;
    }

    private void Update()
    {
        if (movePlayer)
            MovePlayer();
        if(moving)
            ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        float t = moveTime / totalTime;
        moveTime += Time.deltaTime;
        float animationFloat = Mathf.LerpUnclamped(lastValue, finalValue, t);
        _animator.SetFloat("Swing", animationFloat);

        if (moveTime > totalTime)
        {
            moveTime = 0f;
            lastValue = animationFloat;
            finalValue *= -1;
            enabled = false;
            StartCoroutine(AfterDelay());
        }
    }

    private void ActivateMovePlayer()
    {
        movePlayer = true;
        m_Platform.SetMoving(true);
    }

    private void MovePlayer()
    {
        if (finalValue > 0) m_Platform.movementMod = Vector3.left;
        else m_Platform.movementMod = Vector3.right;

        m_Platform.speed = moveSpeed * (4 / totalTime);
    }

    [SerializeField] float stillDelay;
    IEnumerator AfterDelay()
    {
        Stop();
        float elapsedTime = 0f;
        while(elapsedTime < stillDelay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enabled = true;
    }

    public void Stop()
    {
        m_Platform.movementMod = Vector3.zero;
    }
}
