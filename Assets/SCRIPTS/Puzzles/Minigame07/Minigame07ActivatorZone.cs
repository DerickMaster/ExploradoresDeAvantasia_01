using UnityEngine;

public class Minigame07ActivatorZone : MonoBehaviour
{
    private Minigame07Manager manager;

    private void Start()
    {
        manager = FindObjectOfType<Minigame07Manager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        manager.enabled = true;
        manager.InitialSpawn();
    }
}
