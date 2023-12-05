using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FormGateBehaviour : InteractableObject
{
    Animator _animator;
    int curAmount = 0;
    Material[] m_Materials;

    [SerializeField] int expectedAmount;
    [SerializeField] Material[] _activeMaterials;

    [HideInInspector]public UnityEvent gateOpened;
    [HideInInspector] public UnityEvent _objectReceived;

    new private void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        m_Materials = GetComponentInChildren<Renderer>().materials;
    }

    public override void Interact(InteractionController interactor)
    {
        _animator.SetTrigger("Received");
        _objectReceived.Invoke();
        curAmount++;
        SwitchMaterial(interactor.heldObject.GetComponent<GrabbableObject>().objectID);
        if (curAmount >= expectedAmount)
        {
            _animator.SetTrigger("Open");
            gateOpened.Invoke();
        }
    }

    private void SwitchMaterial(int objId)
    {
        int id = -1;
        switch (objId)
        {
            case 101:
                id = 1;
                break;
            case 102:
                id = 2;
                break;
            case 103:
                id = 3;
                break;
            case 104:
                id = 4;
                break;
            default:
                break;
        }

        if(id != -1)
        {
            m_Materials[id] = _activeMaterials[id-1];
            GetComponentInChildren<Renderer>().materials = m_Materials;
        }
            
    }
}
