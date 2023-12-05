
using UnityEngine;

public class AreaSoundTrigger : TriggerZone
{
    public int _stepKindValue;
    public override void Triggered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerSoundManager>().ChangeStep(_stepKindValue);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
