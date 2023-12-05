using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenEffect : MonoBehaviour
{
    public static UIScreenEffect Instance { get; private set; } 
    private Image imgComponent;

    private void Start()
    {
        if(Instance == null)
            Instance = this;

        imgComponent = GetComponent<Image>();
    }

    public void SetScreenEffect(Sprite newEffect, Color effectColor, float time = 0f)
    {
        StartCoroutine(AppearEffect(newEffect, effectColor, time));
    }

    IEnumerator AppearEffect(Sprite newEffect, Color effectColor,float duration)
    {
        imgComponent.sprite = newEffect;

        float elapsedTime = 0f;
        float targetAlpha = effectColor.a;
        float initialAlpha = imgComponent.color.a;
        if (targetAlpha - initialAlpha <= 0.1f) yield break;

        effectColor.a = 0f;

        while(elapsedTime < duration)
        {
            imgComponent.color = effectColor;
            yield return null;
            elapsedTime += Time.deltaTime;
            effectColor.a = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / duration);
        }

        effectColor.a = targetAlpha;
        imgComponent.color = effectColor;
    }

    public void DeactivateScreenEffect(float duration = 0f)
    {
        StartCoroutine(DisappearEffect(duration));
    }

    IEnumerator DisappearEffect(float duration)
    {
        float elapsedTime = 0f;
        float targetAlpha = 0f;
        float initialAlpha = imgComponent.color.a;
        Color colorToPut = new Color();
        colorToPut.a = initialAlpha;

        while (elapsedTime < duration)
        {
            imgComponent.color = colorToPut;
            yield return null;
            elapsedTime += Time.deltaTime;
            colorToPut.a = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / 0.5f);
        }

        colorToPut.a = targetAlpha;
        imgComponent.color = colorToPut;
        imgComponent.sprite = null;
    }
}