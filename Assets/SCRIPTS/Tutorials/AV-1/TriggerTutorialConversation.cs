using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorialConversation : TriggerZone
{
    public GameObject talkTutorialObj;
    public TutorialManager _tutorialManager;
    public InteractableObject tutorialObj;

    bool onMobile = false;
    //[SerializeField] string buttonName;

    public override void Triggered(Collider other)
    {
        Debug.Log(SystemInfo.deviceType.ToString());
        if (SystemInfo.deviceType == DeviceType.Handheld) onMobile = true;
        if (other.CompareTag("Player"))
        {
            talkTutorialObj.SetActive(true);
            if (onMobile) 
            {
                CanvasBehaviour.Instance._uiMobileManager.SetObjOnTopOfBlackScreen("InterBtn", talkTutorialObj);
            } 
        }
        tutorialObj.objInteractedEvent.AddListener(InteractTriggered);
    }

    public void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            talkTutorialObj.SetActive(false);
        }
    }

    public void InteractTriggered()
    {
        talkTutorialObj.SetActive(false);
        Destroy(this);
        if (onMobile) CanvasBehaviour.Instance._uiMobileManager.DeactivateDarkScreen();
    }
}
