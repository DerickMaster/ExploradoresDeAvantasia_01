using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinoBox : MonoBehaviour
{
    Animator _myAnimator;
    public float _thrustForce;
    public float _thrustDuration;

    public void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }
    public void PushBox(Vector3 direction, TinoBoxButtons buttonPressed)
    {
        StartCoroutine(PushMe(direction, buttonPressed));
    }

    public float waitToPush = 0.5f;
    private IEnumerator PushMe(Vector3 direction, TinoBoxButtons buttonPressed)
    {
        yield return new WaitForSeconds(waitToPush);
        Vector3 initialPosition = transform.position;
        float timeElapsed = 0;
        while(timeElapsed < _thrustDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, initialPosition + direction * _thrustForce, timeElapsed/_thrustDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        buttonPressed.ResetLayer();
    }

    public void PlayDestroyAnimation()
    {
        _myAnimator.Play("Death");
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
