using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockedZoneBehaviour : MonoBehaviour
{
    public class BlockTriggered : UnityEvent<BlockedZoneBehaviour>{}

    public int zoneID;

    public StarterAssets.ThirdPersonController _playerController;
    StarterAssets.StarterAssetsInputs _inputs;
    public Vector3 direction;
    public float length = 50f;
    public float duration;
    public BlockTriggered touched;

    public bool allowPassage = false;
    private bool triggered;

    private void Awake()
    {
        touched = new BlockTriggered();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, direction * length, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //triggered = true;
            touched.Invoke(this);
            if (allowPassage) return;
            if (_playerController == null) _playerController = other.gameObject.GetComponentInChildren<StarterAssets.ThirdPersonController>();
            _playerController.SwitchControl(true);
            StartCoroutine(MoveBack(other.gameObject));
        }
    }

    private IEnumerator MoveBack(GameObject player)
    {
        _inputs = player.GetComponent<StarterAssets.StarterAssetsInputs>();
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _inputs.move.x = direction.x;
            _inputs.move.y = direction.z;
            elapsedTime += Time.deltaTime;
            if(elapsedTime > 0.1f) MoveBackReaction();
            yield return null;
        }
        _inputs.move.x = 0;
        _inputs.move.y = 0;
        _playerController.SwitchControl(false);
        //MoveBackReaction();
    }

    public string tempText;
    [SerializeField] float tempTextTime;
    public Sprite _portrait;
    protected virtual void MoveBackReaction()
    {
        if(_portrait != null) CanvasBehaviour.Instance.SetActiveTempText(tempText, tempTextTime, _portrait);
        else CanvasBehaviour.Instance.SetActiveTempText(tempText, tempTextTime);
    }
}
