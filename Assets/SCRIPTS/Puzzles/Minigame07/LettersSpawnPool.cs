using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LettersSpawnPool : MonoBehaviour
{
    [SerializeField] int letterPoolSize;
    [SerializeField] int numberPoolSize;
    [SerializeField] GameObject charObjPrefab;
    [HideInInspector] public UnityEvent poolEmptiedEvent;

    public List<CharObjectBehaviour> LetterPool { get; private set; }
    public List<CharObjectBehaviour> NumberPool { get; private set; }
    void Awake()
    {
        LetterPool = new List<CharObjectBehaviour>(letterPoolSize);
        for (int i = 0; i < letterPoolSize; i++)
        {
            LetterPool.Add(Instantiate(charObjPrefab, transform.position + (Vector3.down * 10f), Quaternion.identity, transform).GetComponent<CharObjectBehaviour>());
            LetterPool[i].gameObject.SetActive(false);
        }

        NumberPool = new List<CharObjectBehaviour>(numberPoolSize);
        for (int i = 0; i < numberPoolSize; i++)
        {
            NumberPool.Add(Instantiate(charObjPrefab, transform.position + (Vector3.down * 10f), Quaternion.identity, transform).GetComponent<CharObjectBehaviour>());
            NumberPool[i].gameObject.SetActive(false);
        }
    }

    private int numberSpawned = 0;
    public CharObjectBehaviour GetObjToSpawn(bool isLetter)
    {
        if (LetterPool.Count == 0) 
        {
            poolEmptiedEvent.Invoke();
            return null;
        } 
        else
        {
            if (isLetter)
            {
                CharObjectBehaviour objToReturn = LetterPool[LetterPool.Count - 1];
                LetterPool.RemoveAt(LetterPool.Count - 1);
                return objToReturn;
            }
            else
            {
                numberSpawned++;
                if (NumberPool.Count - numberSpawned == -1) numberSpawned = 1;
                int id = NumberPool.Count - numberSpawned;

                CharObjectBehaviour objToReturn = NumberPool[id];
                return objToReturn;
            }
        }
    }

    public void IncreaseLetterPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            LetterPool.Add(Instantiate(charObjPrefab, transform.position + (Vector3.down * 10f), Quaternion.identity, transform).GetComponent<CharObjectBehaviour>());
            LetterPool[i].gameObject.SetActive(false);
        }
    }
}
