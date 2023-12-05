using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractTutorial : TriggerZone
{
    public GameObject interactTutorialObj;
    public TutorialManager _tutorialManager;
    public GrabbableObject tutorialObj;

    bool onMobile = false;
    [SerializeField] string buttonName;

    public override void Triggered(Collider other)
    {
        Debug.Log(SystemInfo.deviceType.ToString());
        if (SystemInfo.deviceType == DeviceType.Handheld) onMobile = true;
        if (other.CompareTag("Player"))
        {
            interactTutorialObj.SetActive(true);
            if (onMobile) CanvasBehaviour.Instance._uiMobileManager.SetObjOnTopOfBlackScreen(buttonName);
        }
        tutorialObj.obj_Grab_Drop.AddListener(InteractTriggered);
    }

    public void OnTriggerEnter(Collider other)
    {
        Triggered(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactTutorialObj.SetActive(false);
        }
    }

    public void InteractTriggered(GrabbableObject grabObj)
    {
        interactTutorialObj.SetActive(false);
        Destroy(gameObject);
        if (onMobile) CanvasBehaviour.Instance._uiMobileManager.DeactivateDarkScreen();
    }
}
