using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StageButtonBehaviour : MonoBehaviour, IReceiveSave
{
    [SerializeField]private Image m_Image;
    [SerializeField]private Text m_Text;
    [SerializeField] private GameObject lockScreen;
    public string sceneName;
    public string sceneSlug = "";
    public bool hasIntro;
    public Vector3 initialPos;
    public bool locked;
    public bool hasHardMode;
    public GameObject hardStageImage;
    [HideInInspector] public UnityEvent<StageButtonBehaviour> btnClicked;

    private void Awake()
    {
        initialPos = transform.position;
        _gameScoreInitialPos = _gameScore.GetComponent<RectTransform>().localPosition;
        _gameScoreTargetPos = _gameScore.GetComponent<RectTransform>().localPosition + new Vector3(0,-100,0);
    }

    public void SetStageInformation(StageSelection.StageLoadInformation info)
    {
        m_Image.sprite = info.stageImage;
        m_Text.text = info.stageName;
        sceneName = info.sceneName;
        hasIntro = info.hasIntro;
        hasHardMode = info.hasHardMode;
        sceneSlug = info.sceneSlug;
        LoadGameScore();
    }

    public void EnterHardMode()
    {
        if (hasHardMode)
        {
            hardStageImage.SetActive(true);
            m_Text.text += " Travessura";
            sceneName += "_Hard";
        }
    }
    public void LeaveHardMode()
    {
        if (hasHardMode)
        {
            hardStageImage.SetActive(false);
            m_Text.text = m_Text.text.Replace(" Travessura", "");
            sceneName = sceneName.Replace("_Hard", "");
        }
    }

    #region GameScore

    public Sprite _litStarSprite;
    public Image[] _stars;
    public Text _timerText;
    public Text[] coinsTexts;

    public GameObject _gameScore;
    public Transform _gameScorePos;
    public Vector3 _gameScoreInitialPos, _gameScoreTargetPos;
    [SerializeField] float _movementTime;

    IEnumerator ShowGameScoreCoroutine(bool opening)
    {
        RectTransform gameScoreTrasnform = _gameScore.GetComponent<RectTransform>();

        float elapsedTime = 0f;
        while (elapsedTime < _movementTime)
        {
            yield return null;
            if (opening) gameScoreTrasnform.localPosition = Vector3.Lerp(_gameScoreInitialPos, _gameScoreTargetPos, elapsedTime / _movementTime);
            else gameScoreTrasnform.localPosition = Vector3.Lerp(_gameScoreTargetPos, _gameScoreInitialPos, elapsedTime / _movementTime);
            elapsedTime += Time.deltaTime;
        }
    }

    public void LoadGameScore()
    {
        if (sceneSlug.Length > 1)
            StartCoroutine(APIInterface.GetGamescore(sceneSlug, this));
        else
        {
            Debug.Log("scene slug not set for: " + sceneName);
            GetSaveData(new AdventureManager.AdventureSaveData());
        }
    }

    public void OnClicked()
    {
        if (locked) return;

        btnClicked.Invoke(this);
        StartCoroutine(ShowGameScoreCoroutine(true));
    }

    public void ReturnGameScore()
    {
        StartCoroutine(ShowGameScoreCoroutine(false));
    }

    public void GetSaveData(AdventureManager.AdventureSaveData data)
    {
        if (!data.finished)
        {
            Debug.Log("gamescore for " + sceneName +  " is invalid");
            return;
        }

        for (int i = 0; i < data.stars; i++)
        {
            _stars[i].sprite = _litStarSprite;
        }

        string  minutes = ((int)(data.elapsedTime/60f)).ToString("00");
        string seconds = (data.elapsedTime % 60f).ToString("00");

        _timerText.text = minutes + ":" + seconds;
        string coinsAmount = data.curCoins.ToString("000");

        for (int i = 0; i < coinsTexts.Length; i++)
        {
            coinsTexts[i].text = coinsAmount.Substring(i, 1);
        }
    }

    public void SetActiveLockScreen(bool active)
    {
        lockScreen.SetActive(active);
        locked = active;
        //GetComponentInChildren<Button>().enabled = active;
    }

    #endregion
}
