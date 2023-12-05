using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CCType
{
    public enum Type
    {
        Freeze,
        Root,
        Stun
    }
}

public class DoTBuildUp
{
    public float curValue { private set; get; }
    private readonly float maxValue;
    public int dmg;
    private Coroutine coroutine;

    public DoTBuildUp(float maxValue, int dmg, Coroutine coroutine)
    {
        curValue = 0f;
        this.maxValue = maxValue;
        this.dmg = dmg;
        this.coroutine = coroutine;
    }

    public bool IncreaseBuildUp(float addValue)
    {
        curValue += addValue;

        if (curValue > maxValue) return true;
        else return false;
    }

    public Coroutine GetCoroutine()
    {
        return coroutine;
    }
}

public class HurtBox : MonoBehaviour
{
    public HealthUnit m_HealthUnit;
    public bool IsOpen { get; private set; } = true;
    public bool canSwitch = true;
    public float m_InvencibilityTime;
    Dictionary<string, DoTBuildUp> buildups;

    [SerializeField] GameObject snowmanPrefab;
    private SnowmanEffectBehaviour snowman;
    [HideInInspector] public UnityEvent<GameObject, float> hitTaken;
    [HideInInspector] public UnityEvent<StarterAssets.ActionsAllowed, float> stopAllActions;
    [HideInInspector] public UnityEvent<bool> blockSwitch;

    private void Start()
    {
        m_HealthUnit = GetComponentInParent<HealthUnit>();
        buildups = new Dictionary<string, DoTBuildUp>();
        snowmanPrefab = Resources.Load<GameObject>("Prefabs/Snowman");
    }

    [ContextMenu("TakeDmg")]
    public void TestTakeDmg()
    {
        TakeDamage();
    }


    Coroutine invencibilyCoroutine;
    public void TakeDamage(int damage = 2, GameObject dealer = null, float knockbackForce = 10f)
    {
        if (IsOpen)
        {
            m_HealthUnit.TakeDamage(damage);
            hitTaken.Invoke(dealer, knockbackForce);
            if (m_HealthUnit.curHealthAmount == 0) IsOpen = false;
            else invencibilyCoroutine = StartCoroutine(InvencibilityCountdown());
        }
    }

    public Material _damageMaterial;
    private IEnumerator InvencibilityCountdown()
    {
        if (m_InvencibilityTime <= -1f) yield break;
        IsOpen = false;
        canSwitch = false;
        yield return new WaitForSeconds(m_InvencibilityTime);
        IsOpen = true;
        yield return null;
        canSwitch = true;
        invencibilyCoroutine = null;
    }

    public void SetOpenHurtbox(bool open)
    {
        if (invencibilyCoroutine != null) StopCoroutine(invencibilyCoroutine);
        IsOpen = open;
    }

    public void TakeStun(CCType.Type ccType, float duration)
    {
        switch (ccType)
        {
            case CCType.Type.Freeze:
                FreezeEffect(duration);
                break;
            case CCType.Type.Root:
                break;
            case CCType.Type.Stun:
                break;
        }
    }

    private void FreezeEffect(float duration)
    {
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        if (snowman == null) snowman = Instantiate(snowmanPrefab,gameObject.transform.position, rotation).GetComponent<SnowmanEffectBehaviour>();

        snowman.playerRef = gameObject;
        snowman.transform.position = transform.position;
        snowman.duration = duration;

        snowman.gameObject.SetActive(true);

        StarterAssets.ActionsAllowed actions = new StarterAssets.ActionsAllowed()
        {
            canAttack = false,
            canInteract = false,
            canJump = false,
            canMove = false,
            canSwitch = false
        };
        stopAllActions.Invoke(actions, duration);
    }

    public void TakeBuildUp(string name,float addValue , float dotMaxValue, int dotDmg)
    {
        if (!IsOpen) return;

        if (buildups.ContainsKey(name))
        {
            if (buildups[name].IncreaseBuildUp(addValue))
            {
                TakeDamage(buildups[name].dmg);
                RemoveBuildup(name);
            }
        }
        else
        {
            buildups.Add(name, new DoTBuildUp (dotMaxValue,dotDmg, StartCoroutine(CleanseDelay(name))));
        }
    }

    private float delayBetweenChecks = 0.25f;
    IEnumerator CleanseDelay(string dotName, float delay = 1f)
    {
        yield return null;

        float elapsedTime = 0f;
        float lastValue = buildups[dotName].curValue;
        while(elapsedTime < delay)
        {
            yield return new WaitForSeconds(delayBetweenChecks);
            if (lastValue < buildups[dotName].curValue) 
            {
                elapsedTime = 0f;
                lastValue = buildups[dotName].curValue;
            } 
            else elapsedTime += delayBetweenChecks;
        }

        RemoveBuildup(dotName);
    }

    public void RemoveBuildup(string name)
    {
        if (buildups.ContainsKey(name))
        {
            StopCoroutine(buildups[name].GetCoroutine());
            buildups.Remove(name);
        }
    }
}
