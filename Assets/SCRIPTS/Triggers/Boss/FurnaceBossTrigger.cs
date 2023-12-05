using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceBossTrigger : TriggerZone
{
    public Adventure3BossManager _bossManager;
    public override void Triggered(Collider other)
    {
        _bossManager.StartBossFight();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
