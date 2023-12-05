using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerDirector_ObjActive : TriggerZone
{
    public PlayableDirector timeline;
    public bool happensOnce = true;
    [SerializeField] GameObject targetObj;

    public override void Triggered(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && targetObj && !targetObj.activeSelf)
        {
            timeline.Play();
            if (happensOnce)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider player)
    {
        Triggered(player);
    }
}
