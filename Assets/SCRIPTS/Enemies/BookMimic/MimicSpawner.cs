using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicSpawner : MonoBehaviour
{
    [SerializeField] GameObject _mimicPrefab;
    Collider _myCollider;
    private void Start()
    {
        _myCollider = GetComponent<Collider>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnMimics();
        }
    }

    [SerializeField] int _mimicCount = 3;
    void SpawnMimics()
    {
        while(transform.childCount < _mimicCount)
        {
            GeneratePoint();
        }
    }

    void GeneratePoint()
    {
        var bounds = _myCollider.bounds;
        var px = Random.Range(bounds.min.x, bounds.max.x);
        var py = Random.Range(bounds.min.y, bounds.max.y);
        Vector3 pos = new Vector3(px , 0 ,py);

        Instantiate(_mimicPrefab, transform.position + pos, Quaternion.identity, gameObject.transform);
    }
}
