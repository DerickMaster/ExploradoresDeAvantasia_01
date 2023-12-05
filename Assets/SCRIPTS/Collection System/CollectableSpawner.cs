using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] GameObject collectablePrefab;
    private CollectableBehaviour m_Collectable;

    private void Start()
    {
        //StartSpawnCoroutine();
        m_Collectable = Instantiate(collectablePrefab, transform.position, Quaternion.identity).GetComponent<CollectableBehaviour>();
        m_Collectable._collectedEvent.AddListener(StartSpawnCoroutine);
        m_Collectable.selfDestroy = false;
        m_Collectable.gameObject.SetActive(false);

        Spawn();
    }

    [SerializeField] float _spawnDelay = 10f;
    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(_spawnDelay);
        Spawn();
    }

    public void Spawn()
    {
        m_Collectable.Collected = false;
        m_Collectable.gameObject.SetActive(true);
    }

    public void StartSpawnCoroutine()
    {
        StartCoroutine(SpawnCoroutine());
    }
}
