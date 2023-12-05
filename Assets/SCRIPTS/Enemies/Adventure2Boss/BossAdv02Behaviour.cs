using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Cinemachine;

public class BossAdv02Behaviour : MonoBehaviour
{
    private enum BossFightState
    {
        Active,
        Attacking
    }

    BossFightSpiritBehaviour spirit;
    PlantBossBahaviour plantBoss;
    PoisonDoTArea[] dotAreas;
    BossVineBehaviour[] vines;
    BossFightState state = BossFightState.Active;

    int curAmountDestroyed = 0;
    [SerializeField] int vineAmountPerWave;
    [SerializeField] float delayBetweenAttacks;
    [SerializeField] float delayToAttack;
    [SerializeField] CinemachineVirtualCamera cameraForBoss;
    [SerializeField] PlayableDirector _bossVictoryCin;
    private void Start()
    {
        cameraForBoss = GetComponentInChildren<CinemachineVirtualCamera>();
        spirit = GetComponentInChildren<BossFightSpiritBehaviour>();

        plantBoss = GetComponentInChildren<PlantBossBahaviour>();
        plantBoss.activateDoTArea.AddListener(ActivateDoTZone);
        plantBoss.finishedAtk.AddListener(ReturnToActive);

        dotAreas = GetComponentsInChildren<PoisonDoTArea>();

        vines = GetComponentsInChildren<BossVineBehaviour>();
        foreach (BossVineBehaviour vine in vines)
        {
            vine.destroyedEvent.AddListener(VineDestroyed);
        }
        enabled = false;
    }

    [ContextMenu("Start boss fight")]
    public void StartFight()
    {
        enabled = true;
        cameraForBoss.enabled = true;
        plantBoss.InitiateFight();
        ActivateVines();
    }

    private void Update()
    {
        switch (state)
        {
            case BossFightState.Active:
                ActiveBehaviour();
                break;
            case BossFightState.Attacking:
                break;
        }
    }

    
    private float elapsedTime = 0f;
    private void ActiveBehaviour() 
    {
        if(elapsedTime > delayBetweenAttacks)
        {
            elapsedTime = 0f;
            state = BossFightState.Attacking;
            plantBoss.PrepareAttack();
            SpiritPoint(Random.Range(0, 4));
            StartCoroutine(DelayToAttack());
        }
        elapsedTime += Time.deltaTime;
    }

    private IEnumerator DelayToAttack()
    {
        yield return new WaitForSeconds(delayToAttack);
        plantBoss.UnleashAttack();
    }

    private void ReturnToActive()
    {
        state = BossFightState.Active;
    }

    private void SpiritPoint(int direction)
    {
        if (lastZoneId > -1)
        {
            dotAreas[lastZoneId].DeactivateArea();
            if (lastZoneId == direction)
            {
                direction++;
                if (direction >= dotAreas.Length) direction = 0;
            }
        }
        spirit.ShowDirection(direction + 1);
        lastZoneId = direction;
    }

    int lastZoneId = -1;
    private void ActivateDoTZone()
    {
        dotAreas[lastZoneId].ActivateArea();
    }

    private void VineDestroyed()
    {
        curAmountDestroyed++;
        ReturnToActive();
        ActivateVines();
    }

    private void ActivateVines()
    {
        if ((curAmountDestroyed % vineAmountPerWave) == 0)
        {
            if(curAmountDestroyed > 0) plantBoss.TakeHit();
            if (plantBoss.curHp <= 0)
            {
                EndFight();
            }
            else Invoke(nameof(SpawnVines), 1.5f);
        }
    }

    private void EndFight()
    {
        enabled = false;
        cameraForBoss.enabled = false;
        _bossVictoryCin.Play();
        dotAreas[lastZoneId].gameObject.SetActive(false);
    }

    private void SpawnVines()
    {
        List<int> vinesToActivate = new List<int>();
        do
        {
            int valueToAdd = Random.Range(0, vines.Length);
            if (!vinesToActivate.Contains(valueToAdd)) vinesToActivate.Add(valueToAdd);
        } while (vinesToActivate.Count < vineAmountPerWave);
        foreach (int id in vinesToActivate)
        {
            Debug.Log(id.ToString());
            vines[id].ResetState();
        }
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        throw new System.NotImplementedException();
    }
}