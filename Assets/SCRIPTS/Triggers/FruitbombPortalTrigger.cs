using UnityEngine;

public class FruitbombPortalTrigger : TriggerZone
{
    public int _minigame = 1;
    public override void Triggered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_minigame == 1)
                av1minigamemanager._instance.SendToNextRegion();
            else
                av1minigame2manager._instance.SendToNextRegion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
