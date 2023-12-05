using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBossSupportTrigger : TriggerZone
{
    public BossAdv02Behaviour _bossBhv;

    public override void Triggered(Collider other)
    {
        _bossBhv.StartFight();
    }

    public void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
