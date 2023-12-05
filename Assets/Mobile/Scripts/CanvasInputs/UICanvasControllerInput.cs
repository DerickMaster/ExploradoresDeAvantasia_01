using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualAttackInput(bool virtualAttackState)
        {
            Debug.Log("atk pressionado");
            starterAssetsInputs.AttackInput(virtualAttackState);
        }
        
        public void VirtualInteractInput(bool virtualInteractState)
        {
            starterAssetsInputs.InteractInput(virtualInteractState);
        }
        
        public void VirtualAdvanceDialogueInput(bool virtualAdvanceDialogue)
        {
            starterAssetsInputs.AdvanceDialogueInput(virtualAdvanceDialogue);
        }
        
    }

}
