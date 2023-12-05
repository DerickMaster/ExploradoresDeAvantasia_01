using UnityEngine;

namespace Challenge_09
{
    public class InitializeHandNumberFromDoor : MonoBehaviour
    {
        private void Start()
        {
            try
            {
                GetComponentInChildren<LockedSyllabsGateBehaviour>().puzzleFinishedEvent.AddListener(SetValue);
            }
            catch (System.NullReferenceException)
            {
                Debug.Log("couldnt find LockedSyllabsGateBehaviour in children");
            }
        }

        private void SetValue(LockedSyllabsGateBehaviour obj)
        {
            int value = obj.GetSyllabsAmount();
            GetComponentInChildren<ZaHandoBehaviour>().SetHandValue(value);
            GetComponentInChildren<NumberSignBehaviour>().ActivateSign(value);
            transform.parent.GetComponentInChildren<EnergyControllerManager>().expectedValue = value;
        }
    }
}

