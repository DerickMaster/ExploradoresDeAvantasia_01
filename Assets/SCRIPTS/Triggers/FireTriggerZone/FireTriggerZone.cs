using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerZone : MonoBehaviour
{
    IEnumerator _vanishCoroutine;
    private void Start()
    {
        _vanishCoroutine = ClearFire();
        StartCoroutine(_vanishCoroutine);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            other.GetComponentInChildren<HurtBox>().TakeDamage();
        }
    }
    
    public void DontVanish()
    {
        StopCoroutine(_vanishCoroutine);
    }

    [SerializeField] float _fireTimer = 3f;
    private IEnumerator ClearFire()
    {
        yield return new WaitForSeconds(_fireTimer);
        Destroy(gameObject);
    }
}
