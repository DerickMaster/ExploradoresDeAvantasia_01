using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 moveDirection;
		public Vector2 look;
		public Vector2 touchPosition;

		public bool jump;
		public bool sprint;
		public bool interact;
		public bool submit;
		public bool cancel;
		public bool attack;
		public bool keepAttacking;
		public bool touchPressed;

		public float attackHold;
		public float mouseHold;

		public bool dialoguePlaying;
		public bool advanceDialogue;
		public bool pausePressed;

		public bool changePressed;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        private void OnEnable()
        {
			//EnhancedTouch.Touch.onFingerMove += TouchPressed;
        }

		public void ActivateTouchDetection(bool active)
        {
            if (active)
            {
				EnhancedTouch.EnhancedTouchSupport.Enable();

				EnhancedTouch.Touch.onFingerDown += TouchPressed;
				EnhancedTouch.Touch.onFingerMove += TouchDrag;
				EnhancedTouch.Touch.onFingerUp += TouchLetgo;
			}
            else
            {
				EnhancedTouch.EnhancedTouchSupport.Disable();

				EnhancedTouch.Touch.onFingerDown -= TouchPressed;
				EnhancedTouch.Touch.onFingerMove -= TouchDrag;
				EnhancedTouch.Touch.onFingerUp -= TouchLetgo;
			}
		}

		private void TouchPressed(EnhancedTouch.Finger finger)
        {
			mouseHold = 1f;
			touchPosition = finger.screenPosition;
		}

		private void TouchDrag(EnhancedTouch.Finger finger)
        {
			touchPosition = finger.screenPosition;
		}

		private void TouchLetgo(EnhancedTouch.Finger finger)
        {
			mouseHold = 0f;
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}
		
		public void OnMoveDirection(InputValue value)
		{
			MoveDirectionInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnTouchPosition(InputValue value)
		{
			TouchPositionInput(value.Get<Vector2>());
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
        {
			InteractInput(value.isPressed);
        }
		
		public void OnSubmit(InputValue value)
        {
			SubmitInput(value.isPressed);
        }

		public void OnCancel(InputValue value)
        {
			CancelInput(value.isPressed);
        }

		public void OnAttack(InputValue value)
		{
			AttackInput(value.isPressed);
		}
		
		public void OnKeepAttacking(InputValue value)
		{
			KeepAttackingInput(value.isPressed);
		}
		
		public void OnTouchPressed(InputValue value)
		{
			TouchPressedInput(value.isPressed);
		}
		
		public void OnAttackHold(InputValue value)
		{
			AttackHoldInput(value.Get<float>());
		}
		
		public void OnMouseHold(InputValue value)
		{
			MouseHoldInput(value.Get<float>());
		}

		public void OnAdvanceDialogue(InputValue value)
        {
			if (!dialoguePlaying) AdvanceDialogueInput(value.isPressed);
        }

		public void OnPause(InputValue value)
        {
			PauseInput(value.isPressed);
        }

		public void OnChangeCharacter(InputValue value)
        {
			ChangeCharacterInput(value.isPressed);
        }
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}
		
		public void MoveDirectionInput(Vector2 newMoveDirection)
		{
			moveDirection = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void TouchPositionInput(Vector2 newTouchPosition)
		{
			touchPosition = newTouchPosition;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}
		
		public void SubmitInput(bool newInteractState)
		{
			submit = newInteractState;
		}
		
		public void CancelInput(bool newInteractState)
		{
			cancel = newInteractState;
		}

		public void AttackInput(bool newAttackState)
        {
			attack = newAttackState;
        }
		
		public void KeepAttackingInput(bool newAttackState)
        {
			keepAttacking = newAttackState;
        }
		
		public void TouchPressedInput(bool newTouchPressedState)
        {
			touchPressed = newTouchPressedState;
        }
		
		public void AttackHoldInput(float holdValue)
        {
			attackHold = holdValue;
        }
		
		public void MouseHoldInput(float holdValue)
        {
			mouseHold = holdValue;
        }

		public void AdvanceDialogueInput(bool newAdvanceDialogueState)
        {
			advanceDialogue = newAdvanceDialogueState;
		}

		public void PauseInput(bool newPauseState)
        {
			pausePressed = newPauseState;
        }

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void ChangeCharacterInput(bool newChangeState)
        {
			changePressed = newChangeState;
		}
	}
	
}