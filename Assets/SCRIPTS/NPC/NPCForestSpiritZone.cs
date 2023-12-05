using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCForestSpiritZone : TriggerZone
{
    [SerializeField]GameObject _forestSpiritObj;
    NPCForestSpirit _forestSpirit;

    private void Start()
    {
        _forestSpirit = _forestSpiritObj.GetComponent<NPCForestSpirit>();
    }
    public override void Triggered(Collider other)
    {
        _forestSpiritObj.SetActive(true);

    }
    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _forestSpirit.Disappear();
    }
}
