using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUICinematic : MonoBehaviour
{
    public static FadeUICinematic Instance;
    [Header("Scene Fade Settings")]
    public Image _blackout;
    [Range(1.0f, 25.0f)]
    public float _fadeLenght;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    public void Start()
    {
        _blackout.CrossFadeAlpha(0, 0.1f, false);
    }

    [ContextMenu("Test FadeIn")]
    public void FadeIn()
    {
        _blackout.CrossFadeAlpha(1, _fadeLenght, false);
    }

    [ContextMenu("Test FadeOut")]
    public void FadeOut()
    {
        _blackout.CrossFadeAlpha(0, _fadeLenght, false);
    }
}