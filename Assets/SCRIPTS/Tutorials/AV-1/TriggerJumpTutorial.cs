using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerJumpTutorial : MonoBehaviour
{
    private bool onMobile;
    public GameObject JumpTutorialCanvas;
    public TutorialManager _tutorialManager;
    public string mobileTutorialText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                onMobile = true;
            }
            CharacterManager.Instance.GetCurrentCharacter().GetComponent<StarterAssets.ThirdPersonController>().charJumpedEvent.AddListener(CharacterJumped);
            if (onMobile)
            {
                JumpTutorialCanvas.GetComponentInChildren<Text>().text = mobileTutorialText;
                CanvasBehaviour.Instance._uiMobileManager.SetObjOnTopOfBlackScreen("jumpBtn", JumpTutorialCanvas);
            }
            JumpTutorialCanvas.SetActive(true);
            enabled = true;
        }
    }

    private void CharacterJumped()
    {
        try
        {
            JumpTutorialCanvas.SetActive(false);
            if (_tutorialManager != null) _tutorialManager.TutorialTriggered("Jump");
            if (onMobile) CanvasBehaviour.Instance._uiMobileManager.DeactivateDarkScreen();
            Destroy(gameObject);
        }
        catch
        {
            Debug.Log("objeto destruido cancelando trigger de tutorial");
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterManager.Instance.GetCurrentCharacter().GetComponent<StarterAssets.ThirdPersonController>().charJumpedEvent.RemoveListener(CharacterJumped);
            enabled = false;
            JumpTutorialCanvas.SetActive(false);
            if (onMobile) CanvasBehaviour.Instance._uiMobileManager.DeactivateDarkScreen();
        }
    }
}
