using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDifficultyDetector : MonoBehaviour
{
    [HideInInspector] public UnityEvent easyChosenEvent;
    [HideInInspector] public UnityEvent hardChosenEvent;
    [SerializeField] private GameObject FirstBtn;

    private void Start()
    {
        easyChosenEvent.AddListener(DifficultyChosen);
        hardChosenEvent.AddListener(DifficultyChosen);

        StartCoroutine(PauseAfterAFrame());
        CanvasBehaviour.Instance.SwitchControls(true);
        CanvasBehaviour.Instance.pauseBlocked = true;

        EventSystem.current.SetSelectedGameObject(FirstBtn);
    }

    public void EasyBtnClicked()
    {
        easyChosenEvent.Invoke();
    }
    
    public void HardBtnClicked()
    {
        hardChosenEvent.Invoke();
    }

    private void DifficultyChosen()
    {
        Time.timeScale = 1f;
        CanvasBehaviour.Instance.SwitchControls(false);
        CanvasBehaviour.Instance.pauseBlocked = false;
        gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator PauseAfterAFrame()
    {
        yield return null;
        Time.timeScale = 0f;
    }
}