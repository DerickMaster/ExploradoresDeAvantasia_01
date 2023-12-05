using UnityEngine;

public class SlottableObjectBehaviour : InteractableObject
{
    public GameObject HeldObject;
    public GameObject ObjectSlot;

    [SerializeField] protected int keyObjectID;

    [SerializeField] private bool lockable;

    /* se o ID do objeto que vai ser inserido for igual ao ID esperado 
         * chama a funcao para inserir o objeto */
    public override void Interact(InteractionController interactor)
    {
        if (interactor.HoldingObject && interactor.heldObject.GetComponent<InteractableObject>().objectID == keyObjectID)
        {
            ReceiveObject(interactor);
        }
    }

    /* insere o objeto no slot e chama a funcao de bloquear a interacao */
    public void ReceiveObject(InteractionController interactor)
    {
        HeldObject = interactor.PutDownObject();
        HeldObject.transform.SetParent(ObjectSlot.transform);
        HeldObject.transform.localPosition = Vector3.zero;

        if (lockable) LockInteraction();
    }

    public void RemoveObject(InteractionController interactor)
    {
        if(interactor.HoldObject(HeldObject)) HeldObject = null;
    }

    /*impede a interacao mudando a layer do objeto para que nao possa mais ser detectado pelo classe de interagir*/
    public void LockInteraction()
    {
        this.gameObject.layer = LayerMask.GetMask("Default");
    }
}
