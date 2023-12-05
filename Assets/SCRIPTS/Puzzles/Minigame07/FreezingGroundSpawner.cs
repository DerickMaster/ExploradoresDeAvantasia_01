using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingGroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3 scale;

    [ContextMenu("Spawn")]
    public void SpawnObj()
    {
        Instantiate(prefab, transform).transform.localScale = scale;
    }
}
