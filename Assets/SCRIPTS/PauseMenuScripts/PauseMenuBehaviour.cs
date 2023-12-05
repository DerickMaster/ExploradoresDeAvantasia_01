using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenuBehaviour : MonoBehaviour
{
    public StarterAssets.StarterAssetsInputs _input { get; private set; }
    public GameObject firstButton;
    public GameObject ControlsMenu;
    Vector3 oldScale;
    [SerializeField] OptionsPanelController _optionsPanel;
    [SerializeField] GameObject exitConfirmationPanel;


    private void Awake()
    {
        _mytransform = GetComponent<RectTransform>();
        oldScale = _mytransform.localScale;
    }

    private void Update()
    {
        if (_input.pausePressed)
        {
            _input.pausePressed = false;
            _optionsPanel.ShowOptionsScreen(false);
            ContinueButtonClicked();
        }
    }
    private void OnEnable()
    {
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssets.StarterAssetsInputs>();
        PauseGame();
    }

    private void OnDisable()
    {
        exitConfirmationPanel.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        StartCoroutine(PopUpPauseCanvas());
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public RectTransform _mytransform;
    [SerializeField] float totalTimeToAppearPauseCanvas = 1f;
    private IEnumerator PopUpPauseCanvas()
    {
        float timePassed = 0f;
        while (timePassed < totalTimeToAppearPauseCanvas)
        {
            timePassed += Time.unscaledDeltaTime;
            _mytransform.localScale = Vector3.Lerp(Vector3.zero, oldScale, timePassed / totalTimeToAppearPauseCanvas);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

        }
        _mytransform.localScale = oldScale;
    }

    public void ContinueButtonClicked()
    {
        Time.timeScale = 1f;
        CanvasBehaviour.Instance.Activate();
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void StageSelectionButtonClicked()
    {
        Time.timeScale = 1f;
        GameManager.instance.ReturnToStageScreen();
    }

    public void ExitButtonClicked()
    {
        exitConfirmationPanel.SetActive(true);
    }

    public void YesConfirmationBtnClicked()
    {
        Application.Quit();
    }

    public void NoConfirmationBtnClicked()
    {
        exitConfirmationPanel.SetActive(false);
    }

    public void Reload()
    {
        GameManager.instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public SaveSystemManager _saveSystem;
    public void Reset()
    {
        _saveSystem.ResetSave();
        try
        {
            AdventureManager.Instance.ResetSave();
        }
        catch
        {
            Debug.Log("Adventure manager not found, reloading without creating a new save");
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region ControlsMenu

    public void OpenControlsMenu()
    {
        ControlsMenu.SetActive(true);
    }

    #endregion
}