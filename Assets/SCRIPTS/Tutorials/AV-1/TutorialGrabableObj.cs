using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGrabableObj : MonoBehaviour
{
    [SerializeField] GameObject grabObjTutorial;
    [SerializeField] private string buttonName;
    private bool onMobile;
    
    void Start()
    {
        GetComponent<GrabbableObject>().obj_Grab_Drop.AddListener(ObjectGrabbed);
    }

    private void ObjectGrabbed(GrabbableObject grabbableObject)
    {
        grabObjTutorial.SetActive(false);
        Destroy(GetComponents<BoxCollider>()[1]);
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (SystemInfo.deviceType == DeviceType.Handheld) onMobile = true;
            if (other.CompareTag("Player"))
            {
                grabObjTutorial.SetActive(true);
                if (onMobile) CanvasBehaviour.Instance._uiMobileManager.SetObjOnTopOfBlackScreen(buttonName);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SystemInfo.deviceType == DeviceType.Handheld) onMobile = true;
            if (other.CompareTag("Player"))
            {
                grabObjTutorial.SetActive(false);
                if (onMobile) CanvasBehaviour.Instance._uiMobileManager.DeactivateDarkScreen();
            }
        }
    }
}