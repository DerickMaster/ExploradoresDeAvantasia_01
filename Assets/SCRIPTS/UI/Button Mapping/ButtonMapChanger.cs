using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ButtonMapChanger : MonoBehaviour
{
    public string[] actions;
    public Text btnText;
    [HideInInspector] public UnityEvent<string[]> buttonClickedEvent;

    private void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(ButtonClicked);
    }

    public void SetBindingText(InputActionAsset input)
    {
        btnText.text = input.FindAction(actions[0]).bindings[0].ToDisplayString();
    }

    public void ButtonClicked()
    {
        buttonClickedEvent.Invoke(actions);
    }
}
