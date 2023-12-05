using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialConfSelector : MonoBehaviour
{

    public void ExtremeDialogueSpeed()
    {
        PlayerPrefs.SetInt("TextSpeed", 400);
    }

    public void NormalDialogueSpeed()
    {
        PlayerPrefs.SetInt("TextSpeed", 1);
    }

    public GameObject _myCanvas;
    bool open = false;
    public void OpenCanvas()
    {
        open = !open;
        _myCanvas.SetActive(open);
    }

    public void CloseCanvas()
    {
        _myCanvas.SetActive(false);
    }
}
