using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    [SerializeField] ParticleSystem particleRef;

    private void OnTriggerEnter(Collider other)
    {
        particleRef.Play();
    }
}
