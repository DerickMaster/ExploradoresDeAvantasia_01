using System.Collections;
using UnityEngine;

namespace Challenge_09
{
    public class Challenge09RoomManager : MonoBehaviour
    {
        [SerializeField] int energyNeeded;
        [SerializeField] Collider invisibleWall;
        [SerializeField] GameObject bridge;
        [SerializeField] LauraCable mainCable;
        [SerializeField] OperationButtonBehaviour confirmBtn;
        [SerializeField] GameObject rulerParent;
        
        private EnergyRulerBehaviour mainRuler;
        private CompositeEnergyRuler compositeRuler;
        private int rulerTotalValue;
        private EnergyControllerManager[] energyControllers;
        void Start()
        {
            GetComponentInChildren<NumberSignBehaviour>().TriggerChange(energyNeeded);
            compositeRuler = GetComponentInChildren<CompositeEnergyRuler>();
            if (!compositeRuler)
            {
                mainRuler = GetComponentInChildren<EnergyRulerBehaviour>();
                //mainRuler.changeFinishedEvent.AddListener(CheckEnergyRequired);
            }
            energyControllers = GetComponentsInChildren<EnergyControllerManager>();

            foreach (var controller in energyControllers)
            {
                controller.valueChangedEvent.AddListener(UpdateRulerValue);
            }
            try
            {
                confirmBtn.pressedEvent.AddListener(ButtonPressed);
            }
            catch(System.NullReferenceException) { Debug.Log("Confirm button not found"); }
        }

        private void UpdateRulerValue(EnergyControllerManager controller)
        {
            float totalValue = 0;
            foreach (var control in energyControllers)
            {
                totalValue += control.GetEnergyValue();
            }

            rulerTotalValue = (int)totalValue;
            SetRulerValue(totalValue);
        }

        [SerializeField] private int testValue;
        [ContextMenu("Test ")]
        private void DebugTestValue()
        {
            SetRulerValue(testValue);
        }

        private void SetRulerValue(float value)
        {
            if(compositeRuler) compositeRuler.SetValue(value);
            else mainRuler.SetValue(value);
        }

        private void ButtonPressed(int btnMod)
        {
            CheckEnergyRequired();
        }

        private void CheckEnergyRequired()
        {
            if (mainCable) UpdateCable();
            if(rulerTotalValue == energyNeeded)
            {
                StartCoroutine(RaiseBridge());
            }
        }

        private void UpdateCable()
        {
            foreach (var control in energyControllers)
            {
                if (control.GetEnergyValue() <= 0)
                {
                    mainCable.Deactivate();
                    return;
                }
            }
            mainCable.Activate();
        }

        private IEnumerator RaiseBridge()
        {
            foreach (var anim in bridge.GetComponentsInChildren<Animator>())
            {
                anim.SetBool("Active", true);
                yield return new WaitForSeconds(1f);
            }
            invisibleWall.enabled = false;
        }
    }
}

