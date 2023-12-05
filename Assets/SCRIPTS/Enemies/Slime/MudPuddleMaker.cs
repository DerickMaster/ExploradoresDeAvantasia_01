using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudPuddleMaker : MonoBehaviour
{
    [SerializeField] GameObject puddlePrefab;
    [SerializeField] int puddleAmount;
    [SerializeField] LayerMask layer;

    private float elapsedTime;
    [SerializeField] float puddleDelay;

    LinkedList<GameObject> puddles;
    LinkedListNode<GameObject> curPuddle;

    private void Start()
    {
        puddles = new LinkedList<GameObject>();
        for (int i = 0; i < puddleAmount; i++)
        {
            puddles.AddLast(Instantiate(puddlePrefab));
            puddles.Last.Value.SetActive(false);
        }
        curPuddle = puddles.First;
        elapsedTime = puddleDelay + 1f;
    }

    RaycastHit hit;
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        //Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down,Color.green, 2f);
        if(elapsedTime > puddleDelay && Physics.Raycast(new Ray(transform.position + Vector3.up * 0.1f, Vector3.down),out hit, 2f, layer))
        {
            elapsedTime = 0f;
            curPuddle.Value.transform.SetPositionAndRotation(hit.point + Vector3.up * 0.1f, transform.rotation);
            curPuddle.Value.SetActive(true);
            curPuddle = curPuddle.Next ?? puddles.First;
        }
    }
}
