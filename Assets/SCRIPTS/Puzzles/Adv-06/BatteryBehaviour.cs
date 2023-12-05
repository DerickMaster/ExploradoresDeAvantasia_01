using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBehaviour : GrabbableObject
{
    [SerializeField]private int energyValue;

    public int GetEnergyValue()
    {
        return energyValue;
    }
}
