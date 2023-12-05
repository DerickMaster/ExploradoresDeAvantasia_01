using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame06Starter : TriggerZone
{
    [SerializeField]DoorBehaviour[] _doors;

    public override void Triggered(Collider other)
    {
        foreach(DoorBehaviour door in _doors)
        {
            door.CloseDoor();
        }
        StartCoroutine(Delay());
    }

    [SerializeField] float delay = 2f;
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<Minigame06Manager_V1>().StartGame();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Triggered(other); 
    }
}
