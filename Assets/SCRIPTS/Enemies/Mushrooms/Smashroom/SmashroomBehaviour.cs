using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmashroomBehaviour : EnemyBehaviour
{
    private enum State
    {
        Idle,
        Seeking,
        Attacking
    }

    [SerializeField] MoveChain[] moves;
    [SerializeField] int moveId;
    [SerializeField] CircleWarningBehaviour _warningCircle;

    private FollowPlayerBehaviour _followPlayer;
    private CombatUnit _combatUnit;
    private State _State;
    

    private new void Start()
    {
        base.Start();
        _combatUnit = GetComponent<CombatUnit>();
        _followPlayer = GetComponent<FollowPlayerBehaviour>();
        _warningCircle = GetComponentInChildren<CircleWarningBehaviour>();
        _warningCircle.transform.SetParent(null);
        _warningCircle.gameObject.SetActive(false);

        GetComponentInChildren<CombatAnimationsListener>().startCheckingArea.AddListener(_combatUnit.CheckHit);
        GetComponentInChildren<CombatAnimationsListener>().stopCheckingArea.AddListener(_combatUnit.FinishCheckHit);
    }

    [ContextMenu("Startto")]
    private void StartBossFight()
    {
        EnterSeekingBehaviour();
    }

    private void Update()
    {
        switch (_State)
        {
            case State.Idle:
                break;
            case State.Seeking:
                SeekingBehaviour();
                break;
            case State.Attacking:
                break;
        }
    }

    [SerializeField] private float _seekSpeed;
    private void EnterSeekingBehaviour()
    {
        _followPlayer.SetSpeed(_seekSpeed);
        _followPlayer.enabled = true;
        _followPlayer.playerInRangeEvent.AddListener(PlayerOnRange);
        _State = State.Seeking;
    }

    private void SeekingBehaviour()
    {
        _myAnimator.SetFloat("Speed", _followPlayer.GetSpeed());
    }

    private void ExitSeekingBehaviour()
    {
        _followPlayer.enabled = false;
        _followPlayer.playerInRangeEvent.RemoveListener(PlayerOnRange);
    }

    int punchCount;
    private UnityEvent _receivedEvent;
    private void PlayerOnRange()
    {
        ExitSeekingBehaviour();
        _State = State.Attacking;

        _receivedEvent = _combatUnit.UseMove(moves[punchCount]);
        _receivedEvent.AddListener(MoveFinished);
        punchCount++;
        if (punchCount >= 3) punchCount = 0; 
    }

    private void MoveFinished()
    {
        EnterSeekingBehaviour();
        _receivedEvent.RemoveAllListeners();
    }


    [ContextMenu("TestMove")]
    public void TestMove()
    {
        if(moves[moveId].attackNames.Length > 1)
            _combatUnit.UseChain(moves[moveId].attackNames);
        else
            _combatUnit.UseMove(moves[moveId].attackNames[0]);
    }

    public void FinishedJumping()
    {
        _warningCircle.target = CharacterManager.Instance.GetCurrentCharacter();
        _warningCircle.gameObject.SetActive(true);
        _warningCircle.timeToFill = 5f;

        _warningCircle.finishedFillingEvent.AddListener(Falldown);
        Invoke(nameof(LockPosition), 3f);
    }

    private void LockPosition()
    {
        _warningCircle.target = null;
        Vector3 newPos = CharacterManager.Instance.GetCurrentCharacter().transform.position;
        newPos.y = transform.position.y;

        transform.position = newPos;
    }

    [ContextMenu("Jump")]
    public void StartJump()
    {
        _combatUnit.UseMove(moves[3]).AddListener(DeactivateCircle);
    }

    [ContextMenu("Fall")]
    public void Falldown()
    {
        _myAnimator.SetTrigger("Attacking");
    }

    private void DeactivateCircle()
    {
        _warningCircle.gameObject.SetActive(false);
    }
}
