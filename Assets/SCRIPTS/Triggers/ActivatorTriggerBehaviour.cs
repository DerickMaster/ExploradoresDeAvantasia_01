using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe para ligar objetos quando passar, sua �nica fun��o
public class ActivatorTriggerBehaviour : TriggerZone
{
    public GameObject _myObject;

    public override void Triggered(Collider other)
    {
        _myObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }
}
