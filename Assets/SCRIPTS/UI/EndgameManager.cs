using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndgameManager : MonoBehaviour
{
    [Header("References")]


    [Header("Values")]
    public TimerBehaviour _myTimer;
    public UICoinsManager _myCoinsManager;
    public Text _timeElapsed;
    public Text _copperValue;
    public Text _silverValue;
    public Text _goldValue;

    public Image[] _unlitStars;
    public Image[] _litStars;
    public float[] _starCalc;

    AdventureManager.StageType _stageType;
    private void Start()
    {
        try 
        {
            _stageType = FindObjectOfType<AdventureManager>().stageType;
        } catch { Debug.Log("StageInfo not found"); }
        ShowRunResults();
    }


    public void Activate(bool victory)
    {
        if (!victory) DefeatScreen();
        gameObject.SetActive(true);

        ObjectSounds _mySounds = gameObject.GetComponent<ObjectSounds>();
        if (victory) _mySounds.playObjectSound(0);
        else _mySounds.playObjectSound(1);
    }

    public void ShowRunResults()
    {
        GetGoals();
        StarterAssets.ThirdPersonController player = FindObjectOfType<StarterAssets.ThirdPersonController>();
        player.SwitchControl(true);
        //_myCoinsManager = FindObjectOfType<UICoinsManager>();
        if(!_defeated)CalculateStars();
        _timeElapsed.text = ((int)_myTimer.ElapsedTime / 60).ToString() + ":" + ReturnSeconds(_myTimer.ElapsedTime % 60);
        _copperValue.text = "x" + _myCoinsManager.GetCoinValue(1).text;
        _silverValue.text = "x" + _myCoinsManager.GetCoinValue(2).text;
        _goldValue.text = "x" + _myCoinsManager.GetCoinValue(3).text;
        Time.timeScale = 0;
        if(_stageType != AdventureManager.StageType.NotSet)
        {
            AdventureManager.Instance.localFinishedRun = true;
            AdventureManager.Instance.SaveGame(-1);
        }
        Time.timeScale = 0;
        StartCoroutine(PopUpCanvas());
    }

    string ReturnSeconds(float seconds)
    {
        string _seconds = "";
        if (seconds < 10) _seconds += "0";
        _seconds += (int)seconds;
        return _seconds;
    }
    void GetGoals()
    {
        TimerBehaviour goals = FindObjectOfType<TimerBehaviour>();
        _starCalc[2] = goals.GetGoal(2);
        _starCalc[1] = goals.GetGoal(1);
        _starCalc[0] = goals.GetGoal(0);
    }
    public void CalculateStars()
    {
        int starsAmount = AdventureManager.Instance.GetStarsAmount();
        for (int i = 0; i < _litStars.Length; i++)
        {
            if(i < starsAmount) _litStars[i].gameObject.SetActive(true);
        }
    }

    public RectTransform _mytrasnform;
    [SerializeField] float totalTimeToAppearCanvas = 0.5f;
    private IEnumerator PopUpCanvas()
    {
        //_mytrasnform = GetComponentInChildren<RectTransform>();

        Vector3 oldScale = _mytrasnform.localScale;
        float timePassed = 0f;
        while (timePassed < totalTimeToAppearCanvas)
        {
            timePassed += Time.unscaledDeltaTime;
            _mytrasnform.localScale = Vector3.Lerp(Vector3.zero, oldScale, timePassed / totalTimeToAppearCanvas);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

        }
        _mytrasnform.localScale = oldScale;
    }

    [SerializeField] GameObject _victoryRibbon, _defeatRibbon, _homeButton, _continueButton, _reloadButton;
    bool _defeated = false;
    public void DefeatScreen()
    {
        _defeated = true;
        _victoryRibbon.SetActive(false);
        _continueButton.SetActive(false);

        _defeatRibbon.SetActive(true);
        _homeButton.SetActive(true);
        _reloadButton.SetActive(true);
    }

    public void ReloadScene()
    {
        GameManager.instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        GameManager.instance.ReturnToStageScreen();
    }
}
