using UnityEngine;
using UnityEngine.Events;

namespace Challenge_09
{
    public class EnergyControllerManager : MonoBehaviour
    {
        EnergyRulerBehaviour ruler;

        [SerializeField] public float expectedValue;
        [SerializeField] OperationButtonBehaviour incBtn;
        [SerializeField] OperationButtonBehaviour decBtn;
        [SerializeField] LauraCable m_Cable;
        [SerializeField] NumberSignBehaviour m_Sign;

        [HideInInspector] public UnityEvent<EnergyControllerManager> valueChangedEvent;
        [HideInInspector] public UnityEvent<int> sendValueEvent;

        private void Start()
        {
            ruler = GetComponentInChildren<EnergyRulerBehaviour>();
            OperationButtonBehaviour[] btns = GetComponentsInChildren<OperationButtonBehaviour>();
            incBtn = btns[0];
            incBtn.pressedEvent.AddListener(ChangeRulerValue);
            decBtn = btns[1];
            decBtn.pressedEvent.AddListener(ChangeRulerValue);
            ruler.changeFinishedEvent.AddListener(SendValueInEvent);

            foreach (NumberSignBehaviour sign in GetComponentsInChildren<NumberSignBehaviour>())
            {
                sendValueEvent.AddListener(sign.TriggerChange);
            }
        }

        private void ChangeRulerValue(int mod)
        {
            ruler.ModifyValue(mod);
        }

        private void SendValueInEvent()
        {
            valueChangedEvent.Invoke(this);
            sendValueEvent.Invoke((int)ruler.CurValue);
            if (m_Cable)
            {
                ModifyCable();
            }
        }

        private void ModifyCable()
        {
            if (ruler.CurValue > 0) m_Cable.Activate();
            else m_Cable.Deactivate();
        }

        public float GetEnergyValue()
        {
            return ruler.CurValue;
        }
    }
}