using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InteractionButtonListManager : MonoBehaviour
{
    public static InteractionButtonListManager instance;
    [SerializeField] GameObject interactableBtnPrefab;
    Dictionary<string, GameObject> buttons;
    [HideInInspector] public UnityEvent<InteractableObjType> _addedButtonEvent;
    [HideInInspector] public UnityEvent<bool> _buttonInRangeEvent;

    private void Awake()
    {
        instance = this;
        buttons = new Dictionary<string, GameObject>(10);
        try
        {
            FindObjectOfType<CharacterManager>().charChanged.AddListener(ClearButtons);
        }
        catch { Debug.Log("Cena sem character manager"); }
    }

    public void CreateButton(InteractableObject objToInteract)
    {
        if (buttons.ContainsKey(objToInteract.InteractableObjName)) return;

        GameObject button = Instantiate(interactableBtnPrefab, gameObject.transform);
        button.GetComponent<InteractButtonBehaviour>().Initialize(objToInteract.gameObject);
        if(!objToInteract.blockInteract) EventSystem.current.SetSelectedGameObject(button);
        button.transform.SetAsFirstSibling();

        buttons.Add(objToInteract.InteractableObjName, button);
        _addedButtonEvent.Invoke(objToInteract.GetInteractionObjType());
        _buttonInRangeEvent.Invoke(true);
    }

    public void RemoveButton(InteractableObject objToInteract)
    {
        GameObject buttonToRemove;
        if (buttons.TryGetValue(objToInteract.InteractableObjName, out buttonToRemove))
        {
            Destroy(buttonToRemove);
            buttons.Remove(objToInteract.InteractableObjName);
        }
        Invoke(nameof(SetFirstObjAsSelected), 0f);
    }

    private void SetFirstObjAsSelected()
    {
        InteractableObjType objType = 0;
        if (transform.childCount > 0)
        {
            EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
            objType = transform.GetChild(0).gameObject.GetComponent<InteractButtonBehaviour>().m_Interactable.GetInteractionObjType();
        }else _buttonInRangeEvent.Invoke(false);
        _addedButtonEvent.Invoke(objType);
    }

    private void ClearButtons()
    {
        foreach(KeyValuePair<string, GameObject> button in buttons)
        {
            Destroy(button.Value);
        }
        buttons.Clear();
    }

    public void UpdateButtonState(InteractableObject objToInteract)
    {
        GameObject buttonToUpdate;
        if (buttons.TryGetValue(objToInteract.InteractableObjName, out buttonToUpdate))
        {
            buttonToUpdate.GetComponent<InteractButtonBehaviour>().Initialize(objToInteract.gameObject);
            SetFirstObjAsSelected();
        }
    }
}