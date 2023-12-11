using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Challenge_10
{
    public class HeavyButtonBehaviour : MonoBehaviour
    {
        TextMeshPro textMesh;
        Animator _Animator;
        HeavyButtonListener listener;
        [HideInInspector] public UnityEvent<string> pressedEvent;

        private void Start()
        {
            textMesh = GetComponentInChildren<TextMeshPro>();
            _Animator = GetComponentInChildren<Animator>();
            listener = GetComponentInChildren<HeavyButtonListener>();
            listener.pressedEvent.AddListener(Press);
        }

        public void SetText(string text)
        {
            _Animator.SetBool("Pressed", false);
            listener.pressed = false;
            textMesh.text = text;
        }

        [ContextMenu("Test")]
        public void Press()
        {
            listener.pressed = true;

            _Animator.SetBool("Pressed", true);
            pressedEvent.Invoke(textMesh.text);
        }

        public void Deactivate()
        {
            _Animator.SetBool("Pressed", true);
            listener.pressed = true;
        }
    }

}