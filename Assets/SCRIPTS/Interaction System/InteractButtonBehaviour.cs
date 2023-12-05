using UnityEngine;
using UnityEngine.UI;

public class InteractButtonBehaviour : MonoBehaviour
{
    public InteractableObject m_Interactable;
    public GameObject imageE;

    public string objName;
    public Text text;

    private void Start()
    {
        //text = GetComponentInChildren<Text>();
    }

    public void Initialize(GameObject objToInteract)
    {
        if (m_Interactable == null) 
        {
            m_Interactable = objToInteract.GetComponent<InteractableObject>();
            objName = m_Interactable.InteractableObjName;
            text.text = objName;
        }

        if (m_Interactable.blockInteract) SetActiveButton(false);
        else 
        {
            SetActiveButton(true);
            GetComponentInChildren<NavigationCheck>().Initialize(this);
        }
    }

    public void SendInteraction()
    {
        if (m_Interactable.blockInteract) return;
        InteractionController.Instance.InteractionTriggered(m_Interactable.gameObject);
        if (m_Interactable.blockInteract) SetActiveButton(false);
    }

    public void SetActiveButton(bool active)
    {
        GetComponentInChildren<Button>().interactable = active;
        SetImage(active);
    }

    public void SetImage(bool active)
    {
        imageE.SetActive(active);
    }
}
