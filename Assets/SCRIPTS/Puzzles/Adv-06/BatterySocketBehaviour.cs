using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class BatterySocketBehaviour : InteractableObject, IGiveEnergy
{
    private bool filled;
    private BatteryBehaviour battery;

    [HideInInspector] public UnityEvent<int> batterySwitched;

    [SerializeField] private GameObject slot;
    [SerializeField] int[] expectedIds;

    public override void Interact(InteractionController interactor)
    {
        if(filled && !interactor.HoldingObject)
        {
            filled = false;
            GrabbableObjectInteractions.GiveItem(interactor, battery.gameObject);
            batterySwitched.Invoke(-(battery.GetEnergyValue()));
        }
        else if(!filled && interactor.HoldingObject && expectedIds.Contains<int>(interactor.heldObject.GetComponent<GrabbableObject>().objectID))
        {
            filled = true;
            battery = GrabbableObjectInteractions.ReceiveItem(interactor, slot).GetComponent<BatteryBehaviour>();
            batterySwitched.Invoke(battery.GetEnergyValue());
        }
    }

    public UnityEvent<int> GetEvent()
    {
        return batterySwitched;
    }
}