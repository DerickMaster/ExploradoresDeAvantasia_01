using UnityEngine;

public class EnergyReceiverBehaviour : MonoBehaviour
{
    private int chargeValue;
    private InteractableObject m_Obj;

    [SerializeField] private int expectedChargeValue;
    [SerializeField] GameObject[] energySources;
    private void Start()
    {
        foreach (GameObject source in energySources)
        {
            source.GetComponent<IGiveEnergy>().GetEvent().AddListener(ModifyChargeValue);
        }
        m_Obj = GetComponent<InteractableObject>();
    }

    private void ModifyChargeValue(int mod)
    {
        chargeValue += mod;
        if (chargeValue >= expectedChargeValue) m_Obj.Interact(null, true);
        else m_Obj.Interact(null, false);
    }
}
