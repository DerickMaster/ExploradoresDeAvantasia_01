using UnityEngine;
using UnityEngine.UI;

public class TriggerMoveTutorial : MonoBehaviour
{
    public bool onMobile = false;
    public GameObject MoveTutorialCanvas;
    private StarterAssets.StarterAssetsInputs playerInput;
    public TutorialManager _tutorialManager;
    public string mobileTutorialText;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            onMobile = true;
        
        playerInput = CharacterManager.Instance.GetCurrentCharacter().GetComponent<StarterAssets.StarterAssetsInputs>();
        
        if(onMobile)
        {
            MoveTutorialCanvas.GetComponentInChildren<Text>().text = mobileTutorialText;
            CanvasBehaviour.Instance._uiMobileManager.SetObjOnTopOfBlackScreen("Stick", MoveTutorialCanvas);
        }

        MoveTutorialCanvas.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.move != Vector2.zero)
        {
            try
            {
                MoveTutorialCanvas.SetActive(false);
                if(_tutorialManager != null)_tutorialManager.TutorialTriggered("Walk");
                Destroy(gameObject);
                CanvasBehaviour.Instance._uiMobileManager.DeactivateDarkScreen();
            }
            catch
            {
                Debug.Log("objeto destruido cancelando trigger de tutorial");
                Destroy(gameObject);
            }
        }
    }
}
