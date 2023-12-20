using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CanvasBehaviour : MonoBehaviour
{
    public static CanvasBehaviour Instance { get; private set; } 
    public StarterAssets.StarterAssetsInputs _input { get; private set; }
    public CharacterManager _charManager { get; private set; }
    public GameObject pauseCanvas;
    public GameObject interactionsButtonList;
    public UIMobileControlsManager _uiMobileManager;
    public GameObject TempText;
    public GameObject PlayerUI;

    public GameObject InGameInputModule;
    public GameObject UIInputModule;
    public GameObject pauseBtn;
    public bool pauseBlocked = false;

    private DeviceType m_type;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        m_type = SystemInfo.deviceType;

        _charManager = FindObjectOfType<CharacterManager>();
        if (!_charManager) _input = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.StarterAssetsInputs>();
        else
        {
            _input = _charManager._curInput;
            _charManager.charChanged.AddListener(UpdateInput);
        }

        if (PlayerPrefs.GetInt("TextSpeed") != 1) ExtremeDialogueSpeed();
        if (PlayerPrefs.GetInt("TouchMove") == 1) EnableTouchMove();

        Time.timeScale = 1f;
        if (m_type == DeviceType.Handheld) _uiMobileManager.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_input.pausePressed)
        {
            _input.pausePressed = false;
            if(!pauseBlocked) ActivatePauseCanvas();
        }
    }

    public void SwitchControls(bool UIControls)
    {
        InGameInputModule.SetActive(!UIControls);
        UIInputModule.SetActive(UIControls);
    }

    public void ActivatePauseCanvas()
    {
        SwitchControls(true);

        pauseCanvas.SetActive(true);

        if (m_type == DeviceType.Handheld)
        {
            _uiMobileManager.gameObject.SetActive(false);
            //pauseBtn.SetActive(false);
        }
        this.gameObject.SetActive(false);

    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
        SwitchControls(false);
        if (m_type == DeviceType.Handheld) _uiMobileManager.gameObject.SetActive(true);
    }

    public void SetActiveDPad(bool active)
    {
        if (m_type == DeviceType.Handheld) _uiMobileManager.SetActiveBoardPuzzleUI(active);
    }

    public void SetActiveBaseMobileUI(bool active)
    {
        if (m_type == DeviceType.Handheld) _uiMobileManager.SetActiveBaseUI(active);
    }

    public void SetActiveButtonList(bool active)
    {
        interactionsButtonList.SetActive(active);
    }
    
    public void SetActivePlayerUI(bool active)
    {
        PlayerUI.SetActive(active);
        if (PlayerPrefs.GetInt("TouchMove") == 1) _touchMoveObj.SetActive(active);
    }

    private Coroutine countDown;
    [SerializeField] Image _tempPortrait;
    [SerializeField] Sprite _defaultPortraitSprite;
    public void SetActiveTempText(string text, float time = 2f, Sprite speaker = null, FMODUnity.EventReference soundToPlay = default)
    {
        if (speaker == null) _tempPortrait.sprite = _defaultPortraitSprite;
        else _tempPortrait.sprite = speaker;

        if(!soundToPlay.IsNull)
        {
            PlayTempTextSound(soundToPlay);
        }

        TempText.SetActive(true);
        TempText.GetComponentInChildren<Text>().text = text;
        if (countDown != null) StopCoroutine(countDown);
        if(time != 0f) countDown = StartCoroutine(TempTextCountDown(time));
    }

    public void DisableTempText()
    {
        TempText.SetActive(false);
    }

    FMODUnity.EventReference currentDialogueTrack;
    FMOD.Studio.EventInstance _dialogueTrack;
    private void PlayTempTextSound(FMODUnity.EventReference soundToPlay)
    {
        if (!soundToPlay.IsNull)
        {
            currentDialogueTrack = soundToPlay;
            _dialogueTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            _dialogueTrack = FMODUnity.RuntimeManager.CreateInstance(currentDialogueTrack);
            _dialogueTrack.start();
        }
    }
    
    public void SetActiveTempText(string[] texts, float time, int curText = 0)
    {
        TempText.SetActive(true);
        TempText.GetComponentInChildren<Text>().text = texts[curText];
        StartCoroutine(MultiTextCountdown(time, texts, curText + 1));
    }

    float elapsedTime = 0f;
    private IEnumerator TempTextCountDown(float activeTime)
    {
        elapsedTime = 0f;
        while (elapsedTime < activeTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        TempText.SetActive(false);
    }

    private IEnumerator MultiTextCountdown(float activeTime, string[] texts, int curText)
    {
        elapsedTime = 0f;
        while (elapsedTime < activeTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        if (curText == texts.Length) TempText.SetActive(false);
        else SetActiveTempText(texts, activeTime, curText);
    }

    [SerializeField] GameObject _touchMoveObj;
    private void EnableTouchMove()
    {
        _touchMoveObj.SetActive(true);
    }

    private void UpdateInput()
    {
        _input = _charManager._curInput;
    }

    public void ExtremeDialogueSpeed()
    {
        GetComponentInChildren<DialogueBoxManager>().textSpd = 400;
    }

    #region EndGame

    [SerializeField] EndgameManager _endgame;
    [SerializeField] ParticleSystem _confetti;

    public void ActivateEndgameScreen(bool victory = true, float delay = 0)
    {
        SetActivePlayerUI(false);
        SetActiveBaseMobileUI(false);
        if (!victory) _confetti.gameObject.SetActive(false);
        StartCoroutine(AppearDelay(victory, delay));
    }
    private IEnumerator AppearDelay(bool victory, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _endgame.Activate(victory);
        _confetti.Play();
    }

    public void Reload()
    {
        GameManager.instance.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void StageSelectionButtonClicked()
    {
        Time.timeScale = 1f;
        GameManager.instance.ReturnToStageScreen();
    }

    public void FinishScene()
    {
        GameManager.instance.FinishStage();
    }
    #endregion
}
