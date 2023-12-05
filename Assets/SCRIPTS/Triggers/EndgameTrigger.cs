using UnityEngine.UI;
using UnityEngine;

public class EndgameTrigger : TriggerZone
{

    public override void Triggered(Collider other)
    {
        CanvasBehaviour.Instance.ActivateEndgameScreen();
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
