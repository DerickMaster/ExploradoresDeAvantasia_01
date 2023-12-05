using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FaderUIController : MonoBehaviour
{
    public static FaderUIController instance;

    public Image _blackout;
    
    [Range(1.0f, 25.0f)]
    public float _fadeDuration;

    [HideInInspector] public UnityEvent _fadeEnded;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeOut()
    {
        StartCoroutine(InvokeFadeEvent(true));
        _blackout.gameObject.SetActive(true); _blackout.CrossFadeAlpha(0, 0, false);
        _blackout.CrossFadeAlpha(1, _fadeDuration, false);
    }

    public void FadeIn()
    {
        StartCoroutine(InvokeFadeEvent(false));
        _blackout.gameObject.SetActive(true); _blackout.CrossFadeAlpha(1, 0, false);
        _blackout.CrossFadeAlpha(0, _fadeDuration, false);
    }

    private IEnumerator InvokeFadeEvent(bool fadein)
    {
        yield return new WaitForSeconds(_fadeDuration);
        _fadeEnded.Invoke();
    }
}
