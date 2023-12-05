using System.Collections;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] float timePerSpawn;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] int spawnedObjId;
    bool first = true;
    public void SpawnRocks(int amount)
    {
        if (first) 
        {
            initialAmount = amount;
            first = false;
        } 
        StartCoroutine(SpawnOverTime(amount));
    }

    IEnumerator SpawnOverTime(int amount)
    {
        int curAmount = 0;
        while(curAmount < amount)
        {
            Vector3 offset = new Vector3(Random.Range(0.5f, 2f), Random.Range(0.5f, 2f), Random.Range(0.5f, 2f));
            if (spawnedObjId != 0) Instantiate(rockPrefab, transform.position + offset, Quaternion.identity, transform).GetComponent<GrabbableObject>().objectID = spawnedObjId;
            else Instantiate(rockPrefab, transform.position + offset, Quaternion.identity, transform);
            curAmount++;
            yield return new WaitForSeconds(timePerSpawn);
        }
    }

    int initialAmount;
    public void CheckRespawnRock()
    {
        int count = 0;
        foreach (GrabbableObject rock in GetComponentsInChildren<GrabbableObject>())
        {
            count++;
        }
        if(count < initialAmount / 2)
        {
            SpawnRocks(2);
        }
    }
}
