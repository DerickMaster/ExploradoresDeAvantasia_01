using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SceneInformation
{
    public string sceneName;
    public bool has_Intro;
    public bool started;
}

public class SceneSelectionManager : MonoBehaviour, ISaveable
{
    public static SceneSelectionManager Instance { get; private set; }

    public string confirmationText;

    public GameObject confirmationWindow;
    public Text confirmationWindowText;

    [SerializeField] private SceneInformation[] scenesInfo;

    public GameObject sceneBtnPrefab;
    public SelectSceneButton selectedButton;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        confirmationWindowText = confirmationWindow.GetComponentInChildren<Text>();

        //SaveSystemManager.Instance.Load();
        try
        {
            int count = 0;
            foreach (SceneInformation sceneInfo in scenesInfo)
            {
                SelectSceneButton newBtn = Instantiate(sceneBtnPrefab, gameObject.transform).GetComponent<SelectSceneButton>();
                newBtn.Initialize(sceneInfo, count);
                if (count == 0) EventSystem.current.SetSelectedGameObject(newBtn.gameObject);
                count++;
            }
        }
        catch
        {
            Debug.Log("criacao de butao falhou");
        }

        
    }

    public void ButtonClicked(SelectSceneButton button)
    {
        confirmationWindow.SetActive(true);
        confirmationWindowText.text = confirmationText + button.sceneName + " ?";
        selectedButton = button;
    }

    public void YesButtonClicked()
    {
        LoadNextScene();
    }

    public void NoButtonClicked()
    {
        confirmationWindow.SetActive(false);
        selectedButton = null;
    }

    public void ReturnButtonPressed()
    {
        GameManager.instance.LoadScene("MainScreen");
    }

    private void LoadNextScene()
    {
        if (selectedButton.started || !selectedButton.has_Intro) LoadScene(selectedButton.sceneName);
        else
        {
            GameManager.instance._cinematicInfo = new GameManager.CinematicInfo { _sceneId = selectedButton.sceneName, _opening = true };
            scenesInfo[selectedButton.id].started = true;
            selectedButton.started = true;
            LoadScene("BookStoryTeller");
        }
    }

    private void LoadScene(string sceneName)
    {
        //SaveSystemManager.Instance.Save();
        SceneManager.LoadScene(sceneName);
    }


    public object CaptureState()
    {
        SceneSelectionData sceneSelectData = new SceneSelectionData();
        sceneSelectData.scenesInformation = new SceneInformation[scenesInfo.Length];
        for (int i = 0; i < scenesInfo.Length; i++)
        {
            sceneSelectData.scenesInformation[i].sceneName = scenesInfo[i].sceneName;
            sceneSelectData.scenesInformation[i].started = scenesInfo[i].started;
            sceneSelectData.scenesInformation[i].has_Intro = scenesInfo[i].has_Intro;
        }

        return sceneSelectData;
    }

    public void RestoreState(object state)
    {
        SceneSelectionData data = (SceneSelectionData)state;

        for (int i = 0; i < scenesInfo.Length; i++)
        {
            scenesInfo[i].sceneName = data.scenesInformation[i].sceneName;
            scenesInfo[i].started = data.scenesInformation[i].started;
            scenesInfo[i].has_Intro = data.scenesInformation[i].has_Intro;
        }
    }

    [System.Serializable]
    private struct SceneSelectionData
    {
        public SceneInformation[] scenesInformation;
    }
}
