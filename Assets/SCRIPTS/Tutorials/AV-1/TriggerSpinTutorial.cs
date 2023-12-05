using UnityEngine;
using UnityEngine.UI;

public class TriggerSpinTutorial : TriggerZone
{
    public GameObject spinTutorialCanvas;
    public TutorialManager _tutorialManager;
    public string mobileTutorialText;

    bool onMobile = false;
    [SerializeField] string buttonName;

    public override void Triggered(Collider other)
    {
        if (SystemInfo.deviceType == DeviceType.Handheld) onMobile = true;
        if (other.CompareTag("Player"))
        {
            spinTutorialCanvas.SetActive(true);
            if (onMobile) 
            {
                spinTutorialCanvas.GetComponentInChildren<Text>().text = mobileTutorialText;
                CanvasBehaviour.Instance._uiMobileManager.SetObjOnTopOfBlackScreen(buttonName, spinTutorialCanvas);
            } 
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponentInChildren<AttackManager>().attackEvent.AddListener(AttackTriggered);
        Triggered(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spinTutorialCanvas.SetActive(false);
        }
    }

    public void AttackTriggered()
    {
        spinTutorialCanvas.SetActive(false);
        //_tutorialManager.TutorialTriggered("Spin");
        Destroy(gameObject);
        if (onMobile) CanvasBehaviour.Instance._uiMobileManager.DeactivateDarkScreen();
    }
}
