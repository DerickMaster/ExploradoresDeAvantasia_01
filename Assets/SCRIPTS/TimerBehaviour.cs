using UnityEngine;
using UnityEngine.Playables;

public class TimerBehaviour : MonoBehaviour
{
    [System.Serializable]
    private struct TimeGoals
    {
        public float _1stGoal;
        public float _2ndGoal;
        public float _3rdGoal;
    }

    [SerializeField] private TimeGoals m_goals;
    public float ElapsedTime = 0f;

    private void Awake()
    {
        foreach (var playable in FindObjectsOfType<PlayableDirector>())
        {
            playable.played += PauseTimer;
            playable.stopped += ResumeTimer;
        }
    }

    void Update()
    {
        ElapsedTime += Time.deltaTime;
    }

    public void PauseTimer()
    {
        enabled = false;
    }

    private void PauseTimer(PlayableDirector playable)
    {
        PauseTimer();
    }

    public void ResumeTimer()
    {
        enabled = true;
    }

    private void ResumeTimer(PlayableDirector playable)
    {
        ResumeTimer();
    }

    public float GetGoal(int index)
    {
        if (index == 0)
        {
            return m_goals._1stGoal;
        }
        else if (index == 1)
        {
            return m_goals._2ndGoal;
        }
        else return m_goals._3rdGoal;
    }

    public int GetStarsAmount()
    {
        if (ElapsedTime <= m_goals._3rdGoal) return 3;
        else if (ElapsedTime <= m_goals._2ndGoal) return 2;
        else if (ElapsedTime <= m_goals._1stGoal) return 1;
        else return 0;
    }
}