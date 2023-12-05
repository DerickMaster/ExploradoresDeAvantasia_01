using UnityEngine;
using UnityEngine.UI;

public class TempTextTriggerZone : TriggerZone
{
    [SerializeField] Sprite _portrait;
    [SerializeField] float _duration = 2f;
    [SerializeField, TextArea] string _text;
    [SerializeField] FMODUnity.EventReference _dub;
    public override void Triggered(Collider other)
    {
        CanvasBehaviour.Instance.SetActiveTempText(_text, _duration ,_portrait, _dub);
        if (_happensOnce) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
