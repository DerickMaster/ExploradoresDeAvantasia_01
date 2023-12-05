using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ButtonDirectionMapChanger : MonoBehaviour
{
    public string[] actions;
    public int id;
    public Text btnText;
    [HideInInspector] public UnityEvent<string[], int> buttonClickedEvent;

    private void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(ButtonClicked);
    }

    public void SetBindingText(InputActionAsset input)
    {
        btnText.text = input.FindAction(actions[0]).bindings[id].ToDisplayString();
    }

    public void ButtonClicked()
    {
        buttonClickedEvent.Invoke(actions,id);
    }
}
