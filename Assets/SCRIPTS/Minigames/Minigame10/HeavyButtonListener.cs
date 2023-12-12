using UnityEngine;
using UnityEngine.Events;

namespace Challenge_10
{
    public class HeavyButtonListener : MonoBehaviour
    {
        public bool pressed;
        [HideInInspector] public UnityEvent pressedEvent;

        public void Press()
        {
            if (pressed) return;

            pressedEvent.Invoke();
        }
    }
}

