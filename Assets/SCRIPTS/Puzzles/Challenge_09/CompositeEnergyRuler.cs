using UnityEngine;
using System.Collections ;

public class CompositeEnergyRuler : MonoBehaviour
{
    [SerializeField] float targetValue;
    int[] rulerValues;
    float curValue = 0;
    private EnergyRulerBehaviour[] rulers;

    private void Start()
    {
        rulers = GetComponentsInChildren<EnergyRulerBehaviour>();
    }

    [ContextMenu("Debug Test Value")]
    public void TestValue()
    {
        SetValue(targetValue);
    }

    public void SetValue(float newValue)
    {
        // diminui o valor inicial em 1 caso seja um multiplo de 10, necessario para que a quantidade de reguas esteja correta nesses casos
        int modValue = (int)(newValue - 1);
        int rulerAmount = (modValue / 10) + 1;
        rulerValues = new int[rulers.Length];
        for (int i = 0; i < rulerAmount; i++)
        {
            if (i == rulerAmount - 1) 
            {
                rulerValues[i] = (modValue % 10) + 1; // aumenta o valor em 1 para ficar com o valor original
            } 
            else rulerValues[i] = 10;
        }

        targetValue = newValue;

        // escolhe o id inicial e o id final dependendo se o novo valor eh maior ou menor que o atual
        int initId = 0;
        int endId;
        int mod;
        if (curValue > targetValue)
        {
            initId = rulers.Length - 1;
            endId = -1;
            mod = -1;
        }
        else
        {
            endId = rulers.Length;
            mod = 1;
        }
        curValue = 0;
        StartCoroutine(RulersChangeProcess(initId, endId, mod));
    }

    bool finished = false;
    private IEnumerator RulersChangeProcess(int curId, int targetId, int mod)
    {
        while(curId != targetId)
        {
            //reseta a variavel finished, manda o comando de mudanca de valor para a regua atual com o valor calculado anteriormente que esta no msm id
            finished = false;
            rulers[curId].changeFinishedEvent.AddListener(ChangeFinished);
            rulers[curId].SetValue(rulerValues[curId]);
            // espera a regua atual terminar para continuar
            while (!finished)
            {
                yield return null;
            }
            curValue += rulers[curId].CurValue;
            curId += mod;
        }

        AllRulersFinished();
    }

    private void ChangeFinished()
    {
        finished = true;
    }

    private void AllRulersFinished()
    {
        //apos terminar tds as reguas de serem mudadas remove os listeners
        foreach (EnergyRulerBehaviour rulerBehaviour in rulers)
        {
            rulerBehaviour.changeFinishedEvent.RemoveAllListeners();
        }
    }
}
