using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class PlatformLayerManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent<PlatformLayerManager> cycledEvent;
    public int id;
    List<GameObject> platforms;

    private void Start()
    {
        Transform[] temp = GetComponentsInChildren<Transform>();
        platforms = new List<GameObject>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].parent == transform) platforms.Add(temp[i].gameObject);
        }
        enabled = false;
    }

    float elapsedTime = 0f;
    [SerializeField] float delay;
    private void Update()
    {
        if (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            SetSymbols();
            elapsedTime = 0f;
            enabled = false;
            StartCoroutine(DelayToDisappear());
        }
    }

    string sysmbolsString = "$%#@!*abcdefghijklmnpqrstuvwxyz?;:ABCDEFGHIJKLMNPQRSTUVWXYZ^&";
    private int curPlatformId;
    public int curNum;

    private void SetSymbols()
    {
        curPlatformId = Random.Range(0, platforms.Count);

        for (int i = 0; i < platforms.Count; i++)
        {
            if (i == curPlatformId) platforms[i].GetComponentInChildren<TextMeshPro>().text = curNum.ToString();
            else platforms[i].GetComponentInChildren<TextMeshPro>().text = char.ToString(sysmbolsString.ElementAt(Random.Range(0, sysmbolsString.Length)));
        }
        curNum++;
    }

    IEnumerator DelayToDisappear()
    {
        yield return new WaitForSeconds(delay/2);
        for (int i = 0; i < platforms.Count; i++)
        {
            if (i != curPlatformId) platforms[i].SetActive(false);
        }
        yield return new WaitForSeconds(delay / 2);

        cycledEvent.Invoke(this);
    }

    public void ReactivateLayer()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            if (i != curPlatformId) platforms[i].SetActive(true);
        }
        enabled = true;
    }

    public bool playerInside = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
