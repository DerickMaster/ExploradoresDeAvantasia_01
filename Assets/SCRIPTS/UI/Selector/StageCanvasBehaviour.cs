using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCanvasBehaviour : MonoBehaviour
{
    #region Selectors
    [Header ("Selectors")]
    public GameObject _advCanvas, _miniCanvas, _bossCanvas;
    public void ActivateAdvSelector()
    {
        ActivateCanvas("Adventure");
    }

    public void ActivateMiniSelector()
    {
        ActivateCanvas("Minigame");
    }

    public void ActivateBossSelector()
    {
        ActivateCanvas("Boss");
    }

    public GameObject[] _sceneCanvas;
    public void ActivateCanvas(string canvasName)
    {
        for (int i = 0; i < _sceneCanvas.Length; i++)
        {
            if(canvasName == _sceneCanvas[i].gameObject.name)
            {
                _sceneCanvas[i].SetActive(true);
            }
            else
            {
                _sceneCanvas[i].SetActive(false);
            }
        }
    }
    #endregion
    #region Pop-Ups
    [Header ("Pop-Ups")]
    public GameObject _profileCanvas, _settingsCanvas, _trophiesCanvas;
    public void ShowProfile()
    {
        Debug.Log("Profile");
        _profileCanvas.SetActive(true);
    }

    public void ShowSettings()
    {
        Debug.Log("Settings");
        _settingsCanvas.SetActive(true);
    }

    public void ShowTrophies()
    {
        Debug.Log("Trophy");
        _trophiesCanvas.SetActive(true);
    }

    public void ClosePopUps()
    {
        _profileCanvas.SetActive(false);
        _settingsCanvas.SetActive(false);
        _trophiesCanvas.SetActive(false);
    }
    #endregion
}
