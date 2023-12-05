using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePortalTrigger : MonoBehaviour
{
    public DoorBehaviour _myDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _myDoor.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _myDoor.CloseDoor();
        }
    }
}
