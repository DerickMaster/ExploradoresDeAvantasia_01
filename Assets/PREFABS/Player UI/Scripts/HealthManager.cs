using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HpModifiedEvent : UnityEvent<int>
{}
public class HealthManager : HealthUnit, IEndGame
{
    public HpModifiedEvent hpModified;

    private void Awake()
    {
        hpModified = new HpModifiedEvent();
        _endGameEvent = new UnityEvent<string>();
    }

    public override void TakeDamage(int dmgAmount = 2)
    {
        base.TakeDamage(dmgAmount);
        
        hpModified.Invoke(curHealthAmount);

        if (curHealthAmount == 0)
        {
            GetComponent<StarterAssets.ThirdPersonController>().SwitchControl(true);
            GetComponent<Animator>().Play("Death");
            //_endGameEvent.Invoke("ZeroHealth");
            CanvasBehaviour.Instance.ActivateEndgameScreen(false,1f);
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Hit");
        }
    }

    public override void HealDamage(int healAmount = 2)
    {
        base.HealDamage(healAmount);

        hpModified.Invoke(curHealthAmount);
    }

    [HideInInspector] public UnityEvent<string> _endGameEvent;
    public UnityEvent<string> GetEndGameEvent()
    {
        return _endGameEvent;
    }
}
