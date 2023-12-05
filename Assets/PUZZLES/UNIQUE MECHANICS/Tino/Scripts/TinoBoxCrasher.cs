using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinoBoxCrasher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("TinoBox"))
        {
            other.GetComponent<TinoBox>().PlayDestroyAnimation();
        }
    }
}
