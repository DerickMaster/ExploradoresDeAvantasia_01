using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;


public class GameLossUI : MonoBehaviour
{
    public CanvasBehaviour _mainCanvas;
    public GameObject _buttons;
    public GameObject _firstButton;

    void ZeroHealth()
    {
        FadeIntoLoss();
    }

    public void FadeIntoLoss()
    {
        _mainCanvas.SetActivePlayerUI(false);
        _mainCanvas.SwitchControls(true);
        GameManager.instance.FadeOut();
        _buttons.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_firstButton);
    }

    public void TypeOfGO(string type)
    {
        switch (type)
        {
            case ("WrongType"):
                FadeIntoLoss();
                break;
            case ("ZeroHealth"):
                ZeroHealth();
                break;
            default:
                Debug.Log("GO Type not found");
                break;
        }
    }

    [ContextMenu("ReloadScene")]
    public void ResetScene()
    {
        GameManager.instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToStageScreen()
    {
        GameManager.instance.ReturnToStageScreen();
    }
}
