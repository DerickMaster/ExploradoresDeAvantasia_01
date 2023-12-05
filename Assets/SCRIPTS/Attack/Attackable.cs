using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    public EnemyBehaviour _myBehaviour;

    public void Start()
    {
        _myBehaviour = GetComponentInParent<EnemyBehaviour>();
    }

    //Função base para tomar dano
    public virtual void TakeDamage(int damage)
    {
        _myBehaviour.TakeDamage(damage);
    }
}
