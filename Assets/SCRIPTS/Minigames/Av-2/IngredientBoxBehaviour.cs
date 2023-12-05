using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class IngredientBoxBehaviour : InteractableObject
{
    public class BoxInteracted : UnityEvent<IngredientBoxBehaviour> { }

    public GameObject ingredientPrefab;
    public BoxInteracted interacted;
    public int IngredientID;
    public bool blocked = false;

    Animator _myAnimator;

    private void Awake()
    {
        interacted = new BoxInteracted();
        _myAnimator = GetComponent<Animator>();
    }
    public override void Interact(InteractionController interactor)
    {
        interacted.Invoke(this);
        if (!interactor.HoldingObject && !blocked)
        {
            _myAnimator.Play("Open");
            GameObject ingredientToGive = Instantiate(ingredientPrefab);
            ingredientToGive.GetComponent<GrabbableObject>().SetPhysics(false);
            GrabbableObjectInteractions.GiveItem(interactor, ingredientToGive);
            SwitchBlockedState();

            ingredientToGive.GetComponent<GrabbableObject>().obj_Grab_Drop.AddListener(DestroyDroppedIngredient);
        }
        blocked = false;
    }

    public void BlockBox()
    {
        blocked = true;
    }

    public void BlockInteract()
    {
        DeactivateInteractable();
    }

    private void DestroyDroppedIngredient(GrabbableObject droppedObj)
    {
        if (!droppedObj.grabbed)
        {
            StartCoroutine(DestructionDelay(droppedObj.gameObject));
        }
        SwitchBlockedState();
    }

    IEnumerator DestructionDelay(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(obj);
    }
}