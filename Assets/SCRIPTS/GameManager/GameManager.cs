using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public struct CinematicInfo
    {
        public string _sceneId;
        public bool _opening;
    }

    public static GameManager instance;
    public bool _gameStarted = false;

    private TimerBehaviour _timer;

    public CinematicInfo _cinematicInfo;

    public string lastStageCompleted;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        hideMouseCoroutine = StartCoroutine(HideMouseCooldown());
        LoadControlsOverride();
    }

    Vector2 lastMousePosition = Vector2.zero;
    Coroutine hideMouseCoroutine;
    private void Update()
    {
        if (Mouse.current.position.ReadValue() != lastMousePosition)
        {
            StopCoroutine(hideMouseCoroutine);
            Cursor.visible = true;
            hideMouseCoroutine = StartCoroutine(HideMouseCooldown());
        }
        lastMousePosition = Mouse.current.position.ReadValue();
    }

    IEnumerator HideMouseCooldown()
    {
        yield return new WaitForSecondsRealtime(5f);
        Cursor.visible = false;
    }

    private void InitilializeVariables()
    {
        foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<IEndGame>())
        {
            item.GetEndGameEvent().AddListener(GameOverTriggered);
        }

        _timer = FindObjectOfType<TimerBehaviour>();
        lastStageCompleted = "";
    }

    private float savedTime = 0f;
    private void GameOverTriggered(bool victory, float delay)
    {
        savedTime = _timer.ElapsedTime;
        CanvasBehaviour.Instance.ActivateEndgameScreen(victory, delay);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        InitilializeVariables();

        FadeIn();

        string stageType = "";
        try
        {
            stageType = FindObjectOfType<StageInformation>().stageType;
        }
        catch { Debug.Log("Stage information not found"); }

        switch (stageType)
        {
            case "Adventure":
                AdventureLoaded();
                break;
            case "Minigame":
                MinigameLoaded();
                break;
            default:
                break;
        }
    }

    private void AdventureLoaded()
    {
        if (savedTime > 0f)
        {
            _timer.ElapsedTime = savedTime;
            savedTime = 0f;
        }
    }

    private void MinigameLoaded()
    {

    }

    public void FinishStage()
    {
        string stageType = "";
        try
        {
            stageType = FindObjectOfType<StageInformation>().stageType;
        }
        catch { Debug.Log("Stage information not found"); }

        switch (stageType)
        {
            case "Aventura":
                AdventureFinished();
                break;
            case "Minigame":
                MinigameFinished();
                break;
            default:
                Debug.Log("Stage type not identified");
                break;
        }
    }

    private void AdventureFinished()
    {
        lastStageCompleted = SceneManager.GetActiveScene().name;
        _cinematicInfo._sceneId = SceneManager.GetActiveScene().name;
        _cinematicInfo._opening = false;
        LoadScene("BookStoryteller");
    }

    private void MinigameFinished()
    {
        ReturnToStageScreen();
    }

    public void ReturnToStageScreen()
    {
        SceneManager.LoadScene("StageSelector");
    }


    #region FADER
    public void FadeIn()
    {
        FaderUIController.instance.FadeIn();
    }

    public void FadeOut()
    {
        FaderUIController.instance.FadeOut();
    }
    #endregion

    #region Scene Management

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        FadeIn();
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region NETWORK

    //public string ApiU
    public string accessToken;
    public string accountType;

    public void SetToken(string token, string username)
    {
        accessToken = token;
        SetAccountType(username);
    }

    public void SetAccountType(string username)
    {
        if (username.Equals("admin@mail.com"))
        {
            accountType = "admin";
        }
        else accountType = "student";
    }

    public void SaveCredentials(string username, string password)
    {
        PlayerPrefs.SetString("RememberMe", username);
        PlayerPrefs.SetString(username + "Password", password);
    }

    public void DeleteCredentials()
    {
        if(PlayerPrefs.HasKey("RememberMe"))
        {
            PlayerPrefs.DeleteKey(PlayerPrefs.GetString("RememberMe") + "Password");
            PlayerPrefs.DeleteKey("RememberMe");
        }
    }

    #endregion

    #region CONTROL OVERRIDE
    [SerializeField] private InputActionAsset inputAsset;
    private void LoadControlsOverride()
    {
        if (inputAsset)
            ButtonMappingManager.LoadControls(inputAsset);
        else Debug.Log("Input asset not set");
    }
    #endregion
}
