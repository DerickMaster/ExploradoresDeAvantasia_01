using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICoinCounter : MonoBehaviour
{
    [SerializeField] private bool alwaysOnScreen = false;

    private RectTransform m_RTransform;
    private Vector2 initialPos;
    public char amount { private set; get; }
    public Text m_Text;

    private void Start()
    {
        m_RTransform = (transform as RectTransform);

        float onScreenX;
        onScreenX = m_RTransform.anchoredPosition.x;

        if (!alwaysOnScreen) onScreenX *= -1;

        initialPos = new Vector2(onScreenX, m_RTransform.anchoredPosition.y);
        m_RTransform.anchoredPosition = initialPos;
        amount = '0';
    }
    public void SetDurations(float slideDuration, float onScreenDuration)
    {
        this.slideDuration = slideDuration;
        this.onScreenDuration = onScreenDuration;
    }

    private Coroutine slideInCoroutine;
    public void UpdateAmount(char amount)
    {
        this.amount = amount;
        m_Text.text = amount.ToString();
        if (!alwaysOnScreen) 
        {
            onScreenTime = 0f;
            if (slideInCoroutine != null)
            {
                StopCoroutine(slideInCoroutine);
            }
            slideInCoroutine = StartCoroutine(Slide(new Vector2(-initialPos.x, initialPos.y), true));
        }
    }

    private float slideDuration;
    private float onScreenDuration;
    private float onScreenTime = 0f;
    private IEnumerator Slide(Vector2 targetPos, bool slideIn = false, float slideDuration = 0f)
    {
        float time = 0f;
        if (slideDuration < 0.1f) slideDuration = this.slideDuration;
        Vector2 initialPos = m_RTransform.anchoredPosition;

        while (time < slideDuration)
        {
            Vector2 temp = Vector2.Lerp(initialPos, targetPos, time / slideDuration);
            m_RTransform.anchoredPosition = temp;
            time += Time.deltaTime;
            yield return null;
        }
        m_RTransform.anchoredPosition = targetPos;

        if (slideIn)
        {
            while (onScreenTime < onScreenDuration)
            {
                yield return null;
                onScreenTime += Time.deltaTime;
            }
            StartCoroutine(Slide(this.initialPos, false ,slideDuration/2));
        }
    }
}
