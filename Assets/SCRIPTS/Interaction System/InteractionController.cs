using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class InteractionController : MonoBehaviour
{
    public static InteractionController Instance { get; private set; }

    public StarterAssets.StarterAssetsInputs _input { get; private set; }
    public PlayerInput player_input { get; private set; }
    public LayerMask interactionMask;

    public GameObject currentObj = null;

    public GameObject interactableBtnList;
    public GameObject interactableBtnPrefab;

    public bool HoldingObject = false;
    public GameObject grabSlot;
    public GameObject heldObject;

    //Variável para alterar o boleano "Loaded" para mudar a máquina de estados
    public Animator myAnimator;

    public InteractableObjType charType = 0;

    public Vector3 dropOffset;
    public float dropDistance;
    public LayerMask dropLayer;

    public HurtBox _hurtbox;

    private void OnEnable()
    {
        Instance = this;
    }

    private void Start()
    {
        Instance = this;

        _input = GetComponentInParent<StarterAssets.StarterAssetsInputs>();
        player_input = GetComponentInParent<PlayerInput>();
        myAnimator = GetComponentInParent<Animator>();

        _hurtbox = GetComponentInParent<HurtBox>();
        _hurtbox.hitTaken.AddListener(WasHolding);
        //_hurtbox.stopAllActions.AddListener(StunWasHolding);

        try
        {
            interactableBtnList = FindObjectOfType<InteractionButtonListManager>().gameObject;
        }
        catch when(interactableBtnList == null)
        {
            Debug.Log("InteractionBtnsList not found");
        }
    }

    void Update()
    {
        InteractWithObject();
        CheckDropObject();
    }

    private void InteractWithObject()
    {
        if (_input.interact)
        {
            //_input.interact = false;
            try
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                    EventSystem.current.currentSelectedGameObject.GetComponent<InteractButtonBehaviour>().SendInteraction();
                else
                    Debug.Log("Nao existe um objeto para interagir dentro da area");
            }
            catch (System.Exception ex) 
            {
                Debug.Log("excecao encontrada:" + ex.GetType().Name);
                Debug.Log(ex.Message);
            }
        }
    }

    private void CheckDropObject()
    {
        if (_input.interact)
        {
            _input.interact = false;
            if(HoldingObject && !CheckDropArea()) DropObject();
        }
    }

    private bool CheckDropArea()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + dropOffset, transform.forward,out hit, dropDistance, dropLayer))
        {
            Debug.Log(hit.collider.gameObject.name);
            return true;
        }
        else return false;
    }

    public void EnterDialogueState()
    {
        if (player_input.currentActionMap.name.Equals("PlayerControlled")) return;
        player_input.SwitchCurrentActionMap("PlayerInDialogue");
        interactableBtnList.SetActive(false);
        DialogueStarted();
    }

    public void DialogueFinishedPlaying()
    {
        _input.dialoguePlaying = false;
    }

    public void DialogueStarted()
    {
        _input.dialoguePlaying = true;
        _input.advanceDialogue = false;
    }
    
    public void ExitDialogueState()
    {
        if (player_input.currentActionMap.name.Equals("PlayerInDialogue")) player_input.SwitchCurrentActionMap("Player");
        interactableBtnList.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObjType objType = other.gameObject.GetComponent<InteractableObject>().GetInteractionObjType();
        if (objType == InteractableObjType.Default || objType == InteractableObjType.NPC || objType == charType)
        {
            InteractionButtonListManager.instance.CreateButton(other.GetComponent<InteractableObject>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveButtonFromList(other.gameObject.GetComponent<InteractableObject>());
    }

    public void RemoveButtonFromList(InteractableObject interactableObjectLeaving)
    {
        InteractionButtonListManager.instance.RemoveButton(interactableObjectLeaving);
    }

    public void InteractionTriggered(GameObject interactableObj)
    {
        _input.interact = false;
        InteractableObject interacter = interactableObj.GetComponent<InteractableObject>();
        interacter.Interact(this);
        if (interacter.singleInteraction) RemoveButtonFromList(interacter);
        PlayInteractionAnimation(interacter.objType);
        //WaveBack(interacter, interactableObj);
    }

    void WaveBack(InteractableObject interacter, GameObject Other)
    {
        try
        {
            if (interacter.objType == InteractableObjType.NPC) Other.GetComponent<Animator>().SetTrigger("Welcoming");
        }
        catch (System.Exception)
        {
            Debug.Log("Não tem wave back");
            throw;
        }
    }

    private void PlayInteractionAnimation(InteractableObjType objType)
    {
        string animationType = "";
        switch (objType)
        {
            case (InteractableObjType.Default):
                break;
            case (InteractableObjType.NPC):
                animationType = "Welcoming";
                break;
            case (InteractableObjType.Other):
                animationType = "Interacting";
                break;
        }
        if(animationType.Length > 0)GetComponentInParent<Animator>().SetTrigger(animationType);
    }


    string prevActionMap;
    public bool HoldObject(GameObject objectToHold, bool removeFromList = false)
    {
        if (HoldingObject) return false;

        RemoveButtonFromList(objectToHold.GetComponent<InteractableObject>());

        prevActionMap = player_input.currentActionMap.name;
        player_input.SwitchCurrentActionMap("PlayerHolding");

        objectToHold.transform.SetParent(grabSlot.transform, true);
        heldObject = objectToHold;
        heldObject.transform.localPosition = Vector3.zero;

        HoldingObject = true;
        if (objectToHold.GetComponent<GrabbableObject>()._isHeavy) myAnimator.SetBool("LoadedHeavy", true); 
        myAnimator.SetBool("Loaded", true);

        return true;
    }

    public void DropObject()
    {
        myAnimator.SetBool("Loaded", false);
        myAnimator.SetBool("LoadedHeavy", false);

        heldObject.GetComponent<GrabbableObject>().SetPhysics(true);
        heldObject.transform.SetParent(null);

        heldObject.transform.position = gameObject.transform.position + dropOffset + (this.gameObject.transform.forward * dropDistance);
        heldObject.transform.rotation = Quaternion.identity;

        ResetHeldObject();
        if (prevActionMap != "") player_input.SwitchCurrentActionMap(prevActionMap);
        else Debug.Log("action map anterior nao existe");
    }

    public void StunWasHolding(float throwaway)
    {
        WasHolding(null, 0);
    }

    public void WasHolding(GameObject dealer, float intensity)
    {
        if(heldObject != null)
            DropObject();
    }

    public void TriggerPutDown(GameObject refObj)
    {
        // get interactable object reference
        // trigger animation
    }

    public GameObject PutDownObject()
    {
        //call function on saved object to send held object

        myAnimator.SetBool("Loaded", false);
        myAnimator.SetBool("LoadedHeavy", false);
        GameObject objectToPutDown = heldObject;
        ResetHeldObject();

        return objectToPutDown;
    }

    private void ResetHeldObject()
    {
        if(player_input.currentActionMap.name.Equals("PlayerHolding"))
            player_input.SwitchCurrentActionMap("Player");
        heldObject = null;
        HoldingObject = false;
    }

    public void SpecialInteraction()
    {
        myAnimator.SetBool("Special", true);
        StartCoroutine(ResetSpecialAnimation());
    }

    private IEnumerator ResetSpecialAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetBool("Special", false);
    }

    public void SwitchActionMap(string actionMapName = "Player")
    {
        player_input.SwitchCurrentActionMap(actionMapName);
    }

    public void ResetHeldObjRotation()
    {
        heldObject.transform.localEulerAngles = Vector3.zero;
    }
}
