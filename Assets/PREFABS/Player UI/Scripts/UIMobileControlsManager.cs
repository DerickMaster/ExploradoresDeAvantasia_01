using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMobileControlsManager : MonoBehaviour
{
    //public static UIMobileControlsManager Instance { get; private set; }

    [SerializeField] private GameObject LeftStick;
    [SerializeField] private GameObject JumpBtn;
    [SerializeField] private GameObject InteractBtn;
    [SerializeField] private GameObject AtkBtn;
    [SerializeField] private GameObject PauseBtn;
    [SerializeField] private GameObject DirectionalArrows;
    [SerializeField] private GameObject CancelBtn;
    [SerializeField] private GameObject ChangeBtn;

    [SerializeField] private GameObject blackeningScreen;
    public GameObject objToShow;

    // Indexes: Laura / Tino / Bia / Cauã
    public Sprite[] _charactersIcons;
    public Image _curCharacter;

    //Indexes: Luva / Balão / LauraEspecial / TinoEspecial / BiaEspecial / CauaEspecial / Other
    public Sprite[] _interactionIcons;
    public Image _curInteraction;

    private void Start()
    {
        ChangeCharacterIcon();
        CharacterManager.Instance.charChanged.AddListener(ChangeCharacterIcon);
        InteractionButtonListManager.instance._addedButtonEvent.AddListener(ChangeInteractionIcon);
        InteractionButtonListManager.instance._buttonInRangeEvent.AddListener(ChangeInteractionButtonOpacity);
    }

    public void ChangeInteractionButtonOpacity(bool inRange)
    {
        var tempColor = _curInteraction.color;
        if (inRange)
        {
            tempColor.a = 1f;
        }
        else
        {
            tempColor.a = 0.5f;
        }
        _curInteraction.color = tempColor;
    }

    public void ChangeInteractionIcon(InteractableObjType type)
    {
        try
        {
            _curInteraction.sprite = _interactionIcons[(int)type];

        }
        catch (System.IndexOutOfRangeException)
        {
            _curInteraction.sprite = _interactionIcons[0];
        }
    }

    public void ChangeCharacterIcon()
    {
        _curCharacter.sprite = _charactersIcons[CharacterManager.Instance.GetNextCharacterIndex()];
    }

    public void SetActiveBoardPuzzleUI(bool boolean)
    {
        SetActiveBaseUI(!boolean);
        DirectionalArrows.SetActive(boolean);
        CancelBtn.SetActive(boolean);
    }

    public void SetActiveElement(string buttonName, bool active)
    {
        transform.Find(buttonName).gameObject.SetActive(active);
    }

    public void SetActiveBaseUI(bool boolean)
    {
        LeftStick.SetActive(boolean);
        JumpBtn.SetActive(boolean);
        InteractBtn.SetActive(boolean);
        AtkBtn.SetActive(boolean);
        PauseBtn.SetActive(boolean);
        ChangeBtn.SetActive(boolean);
    }

    [ContextMenu("Show obj")]
    public void SetObjOnTopOfBlackScreen(string objName)
    {
        blackeningScreen.SetActive(true);
        blackeningScreen.transform.SetAsLastSibling();
        transform.Find(objName).SetAsLastSibling();
    }

    public void SetObjOnTopOfBlackScreen(string objName, GameObject objToSetOnTop)
    {
        objToSetOnTop.transform.SetParent(transform);
        objToSetOnTop.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        blackeningScreen.SetActive(true);
        blackeningScreen.transform.SetAsLastSibling();
        objToSetOnTop.transform.SetAsLastSibling();
        transform.Find(objName).SetAsLastSibling();
    }

    [ContextMenu("DeactivateBlackScreen")]
    public void DeactivateDarkScreen()
    {
        blackeningScreen.transform.SetAsLastSibling();
        blackeningScreen.SetActive(false);
    }
}
