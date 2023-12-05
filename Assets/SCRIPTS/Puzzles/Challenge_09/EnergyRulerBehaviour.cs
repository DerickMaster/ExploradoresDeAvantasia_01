using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnergyRulerBehaviour : MonoBehaviour
{

    [SerializeField] float changeSpeed;
    [SerializeField] float testValue;
    [SerializeField] float maxValue;

    Coroutine coroutine;
    Animator m_Animator;
    public float CurValue { private set; get; } = 0;
    float targetValue;

    [HideInInspector] public UnityEvent changeFinishedEvent;

    private void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }

    [ContextMenu("Debug set value")]
    public void SetValueTest()
    {
        SetValue(testValue);
    }

    public void SetValue(float value)
    {
        if (value > maxValue) value = maxValue;
        else if (value < 0) value = 0;

        targetValue = value;
        if(coroutine == null)
            coroutine = StartCoroutine(GradualChange());
    }

    public void ModifyValue(int mod)
    {
        SetValue(CurValue + mod);
    }

    IEnumerator GradualChange()
    {
        int mod = 0;
        while (Mathf.Abs(CurValue - targetValue) > 0.1f)
        {
            if (CurValue < targetValue)
                mod = 1;
            else if (CurValue > targetValue)
                mod = -1;

            CurValue += Time.deltaTime * changeSpeed * mod;
            m_Animator.SetFloat("Value", CurValue);
            yield return null;
        }

        CurValue = targetValue;
        m_Animator.SetFloat("Value", CurValue);

        coroutine = null;
        changeFinishedEvent.Invoke();
    }
}
