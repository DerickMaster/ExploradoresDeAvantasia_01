using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LineSize
{
    public bool[] line;
    public int Length => line.Length;
}

public class PieceBehaviour : GrabbableObject
{
    private Animator m_animator;
    [SerializeField] private int expectedId;
    [SerializeField] private int expectedAmount;

    [SerializeField] private LineSize[] matrixSize;

    private int booksAmount = 0;

    private new void Start()
    {
        base.Start();
        m_animator = GetComponentInChildren<Animator>();
    }
    public override void Interact(InteractionController interactor)
    {
        if (booksAmount < expectedAmount && interactor.HoldingObject && interactor.heldObject.GetComponent<GrabbableObject>().objectID == expectedId)
        {
            GrabbableObjectInteractions.SwallowItem(interactor);
            FillBookcase();
        }
        else if (booksAmount == expectedAmount && interactor.HoldObject(this.gameObject, true))
        {
            SetPhysics(false);
        }
    }

    private void FillBookcase()
    {
        booksAmount++;
        m_animator.SetFloat("Blend", booksAmount);
    }

    public LineSize[] GetMatrixSize()
    {
        return matrixSize;
    }
}
