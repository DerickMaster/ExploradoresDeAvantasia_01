using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPortalBehaviour : MonoBehaviour
{
    [SerializeField] int poolSize;
    [SerializeField] GameObject charObjPrefab;
    [SerializeField] private Mesh[] lettersMeshes;
    [SerializeField] private Material wrongLetterMat;
    [SerializeField] private Material letterMat;

    LettersSpawnPool spawnPool;
    List<bool> spawnBacklog;

    void Awake()
    {
        spawnBacklog = new List<bool>();
        spawnPool = transform.root.GetComponentInChildren<LettersSpawnPool>();
    }

    [ContextMenu("SpawnLetter")]
    public void SpawnTest()
    {
        SpawnLetter(true);
    }

    private Coroutine spawningCoroutine;
    public void SpawnLetters(int amount, bool isLetter)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnBacklog.Add(isLetter);
        }

        if(spawningCoroutine == null)
        {
            spawningCoroutine = StartCoroutine(SpawnCoroutine());
        }
    }

    public Animator _portal;
    private IEnumerator SpawnCoroutine()
    {
        while(spawnBacklog.Count > 0)
        {
            SpawnLetter(spawnBacklog[spawnBacklog.Count - 1]);
            spawnBacklog.RemoveAt(spawnBacklog.Count - 1);
            yield return new WaitForSeconds(1f);
        }

        spawningCoroutine = null;
    }

    public bool SpawnLetter(bool isLetter)
    {
        _portal.SetTrigger("Activate");

        CharObjectBehaviour charObj = spawnPool.GetObjToSpawn(isLetter);

        if (charObj == null) return false;

        charObj.isLetter = isLetter;
        charObj.transform.position = transform.position + (transform.forward*2f);

        if (isLetter)
        {
            charObj.GetComponentInChildren<Renderer>().material = letterMat;
        }
        else 
        {
            charObj.GetComponentInChildren<Renderer>().material = wrongLetterMat;
        } 

        charObj.gameObject.SetActive(true);

        return true;
    }
}
