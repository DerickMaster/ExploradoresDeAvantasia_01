using FMODUnity;
using UnityEngine;

public class TriggerFMOD : TriggerZone
{
    public EventReference _mySound;

    public override void Triggered(Collider other)
    {
        RuntimeManager.PlayOneShot(_mySound, transform.position);
        if(_hasText)
            ReminderText();
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }


    public bool _hasText;
    [SerializeField] string tempText;
    [SerializeField] float tempTextTime;
    protected virtual void ReminderText()
    {
        CanvasBehaviour.Instance.SetActiveTempText(tempText, tempTextTime);
    }
}
