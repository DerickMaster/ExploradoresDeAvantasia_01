using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonMappingManager : MonoBehaviour
{
    public InputActionAsset input;
    public GameObject messageObj;
    private ButtonMapChanger[] mapChangers;
    private ButtonDirectionMapChanger[] mapDirChangers;
    private readonly string playerPrefKey = "PlayerControlsOverride";

    private void Awake()
    {
        mapChangers = GetComponentsInChildren<ButtonMapChanger>();
        mapDirChangers = GetComponentsInChildren<ButtonDirectionMapChanger>();
        foreach (ButtonMapChanger btnMapChanger in mapChangers)
        {
            btnMapChanger.buttonClickedEvent.AddListener(ChangeBindings);
            btnMapChanger.SetBindingText(input);
        }
        
        foreach (ButtonDirectionMapChanger btnMapChanger in mapDirChangers)
        {
            btnMapChanger.buttonClickedEvent.AddListener(ChangeDirectionalBindings);
            btnMapChanger.SetBindingText(input);
        }
    }

    private void OnEnable()
    {
        if (CharacterManager.Instance != null)
            CharacterManager.Instance.GetCurrentCharacter().GetComponent<StarterAssets.ThirdPersonController>().SwitchControl(true);
    }

    private void OnDisable()
    {
        if(CharacterManager.Instance != null)
            CharacterManager.Instance.GetCurrentCharacter().GetComponent<StarterAssets.ThirdPersonController>().SwitchControl(false);
    }

    private void ChangeBindings(string[] actions)
    {
        messageObj.SetActive(true);

        foreach (string action in actions)
        {
            var rebindOperation = input.FindAction(action).PerformInteractiveRebinding();
            rebindOperation
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(operation => BindingCanceled(operation))
                .OnComplete(operation => BindingComplete(operation))
                .Start();
        }
    }
    
    private void ChangeDirectionalBindings(string[] actions, int id)
    {
        messageObj.SetActive(true);

        foreach (string action in actions)
        {
            var rebindOperation = input.FindAction(action).PerformInteractiveRebinding();
            rebindOperation
                .WithTargetBinding(id)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(operation => BindingCanceled(operation))
                .OnComplete(operation => BindingComplete(operation))
                .Start();
        }
    }

    private void BindingComplete(InputActionRebindingExtensions.RebindingOperation operation)
    {
        operation.Dispose();
        messageObj.SetActive(false);
        SaveControls();
    }

    private void BindingCanceled(InputActionRebindingExtensions.RebindingOperation operation)
    {
        operation.Dispose();
        messageObj.SetActive(false);
    }

    private void SaveControls()
    {
        PlayerPrefs.SetString(playerPrefKey, input.SaveBindingOverridesAsJson());
    }

    public static void LoadControls(InputActionAsset _input)
    {
        if (PlayerPrefs.HasKey("PlayerControlsOverride"))
            _input.LoadBindingOverridesFromJson(PlayerPrefs.GetString("PlayerControlsOverride"));
        else Debug.Log("no overide found");
    }

    public void ResetOverrides()
    {
        input.RemoveAllBindingOverrides();
        SaveControls();
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}