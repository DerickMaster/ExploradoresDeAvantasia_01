using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPlatformBehaviour : MonoBehaviour
{
    Animator _myAnimator;
    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }

    bool _inCoolDown = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!_inCoolDown)
                StartCoroutine(SpeedUp(FindObjectOfType<CharacterManager>().GetCurrentCharacter().GetComponent<StarterAssets.ThirdPersonController>()));
        }
    }

    public float _targetMvSpeed;
    public float _buffTime = 2f;
    private IEnumerator SpeedUp(StarterAssets.ThirdPersonController playerController)
    {
        _inCoolDown = true;
        _myAnimator.SetTrigger("Active");
        float previousMvSpeed = playerController.MoveSpeed;
        playerController.MoveSpeed = _targetMvSpeed;
        yield return new WaitForSeconds(_buffTime);
        _myAnimator.SetTrigger("Respawn");
        playerController.MoveSpeed = previousMvSpeed;
        _inCoolDown = false;
    }
}
