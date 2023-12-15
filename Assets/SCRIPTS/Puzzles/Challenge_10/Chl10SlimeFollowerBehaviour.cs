using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge_10
{
    public class Chl10SlimeFollowerBehaviour : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Following
        }

        State m_State;

        private int value;
        FollowPlayerBehaviour followBehaviour;
        private void Start()
        {
            followBehaviour = GetComponentInChildren<FollowPlayerBehaviour>();
            followBehaviour.playerInRangeEvent.AddListener(StopFollowing);
        }

        private void Update()
        {
            switch (m_State)
            {
                case State.Idle:
                    IdleBehaviour();
                    break;
            }
        }

        private void IdleBehaviour()
        {
            if(Vector3.Distance(transform.position, CharacterManager.Instance.GetCurrentCharacter().transform.position) > 4f)
            {
                StartFollowing();
            }
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
            followBehaviour.enabled = false;
            m_State = State.Idle;
        }
    }

}
