using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjsZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GrabbableObject grab = other.GetComponent<GrabbableObject>();
        if(grab)
        {
            grab.ManualDestroy();
        }
    }
}
