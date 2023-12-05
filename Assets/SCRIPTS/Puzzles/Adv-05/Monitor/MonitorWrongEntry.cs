using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonitorWrongEntry : MonoBehaviour
{
    [SerializeField] public UnityEvent _wrongEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _wrongEnter.Invoke();
        }
    }
}
