using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Challenge_10
{
    public class Chl10SlimeFollowerBehaviour : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Following
        }

        [SerializeField] float maxSpeed = 1;
        [SerializeField] float maxDistance = 1;

        State m_State;
        int value;
        NavMeshAgent _agent;
        FollowPlayerBehaviour followBehaviour;
        Animator m_Animator;

        private void Start()
        {
            followBehaviour = GetComponentInChildren<FollowPlayerBehaviour>();
            followBehaviour.playerInRangeEvent.AddListener(StopFollowing);
            m_Animator = GetComponentInChildren<Animator>();
            _agent = GetComponentInChildren<NavMeshAgent>();
        }

        private void Update()
        {
            switch (m_State)
            {
                case State.Idle:
                    IdleBehaviour();
                    break;
                case State.Following:
                    FollowingBehaviour();
                    break;
            }
        }

        private void IdleBehaviour()
        {
            if(StaticUtlities.RateLimiter(30) && Vector3.Distance(transform.position, CharacterManager.Instance.GetCurrentCharacter().transform.position) > maxDistance/2)
            {
                StartFollowing();
            }
        }

        float multiplier;
        private void FollowingBehaviour()
        {
            multiplier = Mathf.Lerp(0.2f, 1, (_agent.remainingDistance - _agent.stoppingDistance) / maxDistance);

            _agent.speed = maxSpeed * multiplier;
            m_Animator.SetFloat("Speed", multiplier);
        }

        public void IncreaseValue(int value)
        {
            this.value += value;
        }

        private void StartFollowing()
        {
            followBehaviour.enabled = true;
            m_State = State.Following;
        }

        private void StopFollowing()
        {
            m_Animator.SetFloat("Speed", 0f);

            followBehaviour.enabled = false;
            m_State = State.Idle;
        }
    }

}
