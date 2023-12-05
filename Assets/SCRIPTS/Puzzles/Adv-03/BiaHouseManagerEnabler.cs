using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiaHouseManagerEnabler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BiaHouseManager _manager;
            _manager = FindObjectOfType<BiaHouseManager>();
            _manager.Interact(null);
            Destroy(gameObject);
        }
    }
}
