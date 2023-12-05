using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Minigame08RoomTrigger : TriggerZone
{
    [SerializeField] Minigame08RoomBehaviour _myRoom;
    [HideInInspector] public UnityEvent<Minigame08RoomBehaviour> refRoomEntered;
    public override void Triggered(Collider other)
    {
        refRoomEntered.Invoke(_myRoom);
        _myRoom.CloseEntrance();

        if (_happensOnce) Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) Triggered(other);
    }
}
