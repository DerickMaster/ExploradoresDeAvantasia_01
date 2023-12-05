using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpecificActiationTrigger : TriggerZone
{
    public GameObject _myObject;
    public int _characterIndex;

    public override void Triggered(Collider other)
    {
        _myObject.SetActive(true);
        if (_happensOnce)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((int)other.gameObject.GetComponentInChildren<InteractionController>().charType == _characterIndex)
            Triggered(other);
    }
}
