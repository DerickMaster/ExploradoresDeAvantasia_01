using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<StarterAssets.StarterAssetsInputs>() is var _input && _input != null)
        {
            _input.jump = true;
        }
    }
}
