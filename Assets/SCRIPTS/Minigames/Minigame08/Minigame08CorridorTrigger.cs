using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class Minigame08CorridorTrigger : TriggerZone
{
    [SerializeField] int _zoneIndex;
    [HideInInspector] public UnityEvent<int> refCorridorEntered;
    public override void Triggered(Collider other)
    {
        refCorridorEntered.Invoke(_zoneIndex);
        if (_happensOnce) Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Triggered(other);
    }
}
