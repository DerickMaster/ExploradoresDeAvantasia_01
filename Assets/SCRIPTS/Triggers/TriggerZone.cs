using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerZone : MonoBehaviour
{
    public bool _happensOnce = false;
    //Quando quiser chamar a função do filho, pode usar GetComponent<TriggerZone>().Triggered()
    public virtual void Triggered(Collider other)
    {

    }
}
