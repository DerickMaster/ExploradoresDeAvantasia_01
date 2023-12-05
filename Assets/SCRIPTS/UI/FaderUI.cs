using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FaderUI : MonoBehaviour
{
    [Header ("Scene Fade Settings")]
    public Image _blackout;
    [Range(1.0f, 25.0f)]
    public float _fadeLenght;
    public float _targetAlpha;

    public bool _initial = false;

    public bool happened = false;

    [HideInInspector]public UnityEvent _fadeEnded;

    private void OnEnable()
    {
        if (_initial) { _blackout.gameObject.SetActive(true); _blackout.CrossFadeAlpha(0, 0, false); }
        else { _blackout.gameObject.SetActive(true); _blackout.CrossFadeAlpha(1, 0, false); }
    }

    public void Fade()
    {
        StartCoroutine(ReEnableFader());
        _blackout.CrossFadeAlpha(_targetAlpha, _fadeLenght, false);
    }

    private IEnumerator ReEnableFader()
    {
        yield return new WaitForSeconds(_fadeLenght);
        _fadeEnded.Invoke();
        gameObject.SetActive(false);
    }
}
