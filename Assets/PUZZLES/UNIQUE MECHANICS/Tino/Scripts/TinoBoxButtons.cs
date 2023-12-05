using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinoBoxButtons : InteractableObject
{
    public TinoBox _myBox;
    public Vector3 _myDirection;
    [SerializeField] LayerMask _hitMask;
    public override void Interact(InteractionController interactor)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + _myDirection * _boxDistance, new Vector3(1.5f, 1.5f, 1.5f), Quaternion.identity,_hitMask);
        if (colliders.Length == 0)
        {
            interactor.SpecialInteraction();
            _myBox.PushBox(_myDirection, this);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            foreach(Collider collider in colliders)
            {
                Debug.Log(collider.name);
            }
        }
    }
    
    public void ResetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public float _boxDistance;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position + _myDirection * _boxDistance, new Vector3(3, 3, 3));
    }
}
