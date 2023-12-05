using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame09PuzzleManager : MonoBehaviour
{
    [SerializeField] GameObject _mimicPrefab;
    BookMimicBehaviour[] _room01Mimics, _room02Mimics;
    [SerializeField] Collider _room01Spawner, _room02Spawner;

    private void Start()
    {
        _room01Mimics = _room01Spawner.GetComponentsInChildren<BookMimicBehaviour>();
        _room02Mimics = _room02Spawner.GetComponentsInChildren<BookMimicBehaviour>();
        foreach (var mimic in _room01Mimics)
        {
            mimic._dieEvent.AddListener(SpawnMimicRoom01);
        }
        foreach (var mimic in _room02Mimics)
        {
            mimic._dieEvent.AddListener(SpawnMimicRoom02);
        }
    }

    public void SpawnMimicRoom01()
    {
        StartCoroutine(SpawnDelay(1));
    }

    public void SpawnMimicRoom02()
    {
        StartCoroutine(SpawnDelay(2));
    }

    private IEnumerator SpawnDelay(int room)
    {
        yield return new WaitForSeconds(3f);
        if(room == 1)
            GeneratePoint(_room01Spawner, 1);
        else
            GeneratePoint(_room02Spawner, 2);
    }

    void GeneratePoint(Collider spawnerCollider, int room)
    {
        var bounds = spawnerCollider.bounds;
        var px = Random.Range(bounds.min.x, bounds.max.x);
        var py = Random.Range(bounds.min.y, bounds.max.y);
        Vector3 pos = new Vector3(px, 0, py);

        GameObject newMimic = Instantiate(_mimicPrefab, spawnerCollider.transform.position + pos, Quaternion.identity, spawnerCollider.transform);
        if (room == 1)
            newMimic.GetComponent<BookMimicBehaviour>()._dieEvent.AddListener(SpawnMimicRoom01);
        else
            newMimic.GetComponent<BookMimicBehaviour>()._dieEvent.AddListener(SpawnMimicRoom02);
    }
}