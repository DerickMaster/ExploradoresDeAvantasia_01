using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAuntAv1 : SimpleNPCInteraction
{
    public GameObject _auntDialogCollider;
    public override void Interact(InteractionController interactor)
    {
        _auntDialogCollider.SetActive(false);
        base.Interact(interactor);
    }
}
