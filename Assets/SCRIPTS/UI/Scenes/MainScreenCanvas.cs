using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainScreenCanvas : MonoBehaviour
{
    public GameObject advButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(advButton);
    }
    public void AdventureButtonPressed()
    {
        GameManager.instance.LoadScene("AdventureSelector");
    }
    public void MinigameButtonPressed()
    {
        GameManager.instance.LoadScene("MinigameSelector");
    }

    public void ResetCredentials()
    {
        GameManager.instance.DeleteCredentials();
        SaveSystemManager.Instance.ResetSave();
    }
}
