using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerDirector : TriggerZone
{
    public PlayableDirector timeline;

    public override void Triggered(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            timeline.Play();
            if (_happensOnce)
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
