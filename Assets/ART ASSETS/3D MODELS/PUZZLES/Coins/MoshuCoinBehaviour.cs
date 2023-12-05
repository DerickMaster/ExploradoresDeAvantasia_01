using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoshuCoinBehaviour : GrabbableObject
{
    public DirectionalArrowsBehaviour[] _myArrows;
    public override void Interact(InteractionController interactor)
    {
        base.Interact(interactor);
        foreach(DirectionalArrowsBehaviour arrow in _myArrows)
        {
            arrow.ArrowDirectionRight(true);
        }
    }
}
