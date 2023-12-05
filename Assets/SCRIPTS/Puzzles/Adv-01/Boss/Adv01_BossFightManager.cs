using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Adv01_BossFightManager : MonoBehaviour
{
    BombTreeBehaviour bombTree;
    BombArea bombArea;
    [SerializeField] Camera bossFightCamera;

    FormGateBehaviour gateBehaviour;

    [SerializeField] float delayBetweenAttacks;
    Coroutine attackCoroutine;
    private void Start()
    {
        bombTree = GetComponentInChildren<BombTreeBehaviour>();
        bombArea = GetComponentInChildren<BombArea>();
        gateBehaviour = GetComponentInChildren<FormGateBehaviour>();

        bombTree.throwBombEvent.AddListener(SpawnBombs);
        gateBehaviour.gateOpened.AddListener(StopAttacking);
        gateBehaviour._objectReceived.AddListener(ObjectReceived);
    }

    [ContextMenu("Start Fight")]
    public void StartFight()
    {
        bombTree.WakeUp();
        attackCoroutine = StartCoroutine(AttackLoop());
        bossFightCamera.gameObject.SetActive(true);
    }

    public PlayableDirector _bossVictoryCin;
    [ContextMenu("End Fight")]
    public void StopAttacking()
    {
        bossFightCamera.gameObject.SetActive(false);
        StopCoroutine(attackCoroutine);
        _bossVictoryCin.Play();
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            bombTree.Attack();
            yield return new WaitForSeconds(delayBetweenAttacks);
        }
    }

    private void SpawnBombs()
    {
        bombArea.SpawnBomb();
    }

    private void ObjectReceived()
    {
        bombTree.TakeDamage();
    }
}
