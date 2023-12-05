using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUnit : MonoBehaviour
{
    public int maxHealthAmount;
    public int curHealthAmount;

    [ContextMenu("TakeDamage")]
    public virtual void TakeDamage(int dmgAmount = 2)
    {
        curHealthAmount -= dmgAmount;
        if (curHealthAmount < 0) curHealthAmount = 0;
    }

    public virtual void HealDamage(int healAmount = 2)
    {
        curHealthAmount += healAmount;
        if (curHealthAmount > maxHealthAmount) curHealthAmount = maxHealthAmount;
    }
}