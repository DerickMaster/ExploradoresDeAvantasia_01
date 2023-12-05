using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlimeBehaviour : SlimeBehaviour
{
    IEnumerator _myCoroutine;
    private new void Start()
    {
        base.Start();
        _myCoroutine = SpawnLavaCoroutine();
        StartSpawnCoroutine(true);
    }

    public void StartSpawnCoroutine(bool start)
    {
        if (start) StartCoroutine(_myCoroutine);
        else StopCoroutine(_myCoroutine);
    }

    [SerializeField] float _spawnRate;
    [SerializeField] GameObject _lavaObject;
    private IEnumerator SpawnLavaCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            Instantiate(_lavaObject, transform.position, Quaternion.identity);
        }
    }
}
