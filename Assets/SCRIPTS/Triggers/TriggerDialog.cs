using UnityEngine;

public class TriggerDialog : TriggerZone
{
    public DialogueObject myDialog;

    public override void Triggered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueBoxManager.Instance.PlayDialogue(myDialog, other.gameObject.GetComponentInChildren<InteractionController>());
            if (_happensOnce)
            {
                Destroy(gameObject);
            }
        }   
    }

    public void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
