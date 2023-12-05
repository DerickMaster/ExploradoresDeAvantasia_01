using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionBookInputs : MonoBehaviour
{
    [Header("Selection Book Screen Input Values")]
    public Vector2 navigate;
    public bool confirm;

    public void OnNavigate(InputValue value)
    {
        NavigatelInput(value.Get<Vector2>());
    }

    public void OnConfirm(InputValue value)
    {
        ConfirmInput(value.isPressed);
    }

    public void NavigatelInput(Vector2 newMoveDirection)
    {
        navigate = newMoveDirection;
    }

    public void ConfirmInput(bool newJumpState)
    {
        confirm = newJumpState;
    }
}
