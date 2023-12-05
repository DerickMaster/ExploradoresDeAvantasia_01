using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [System.Serializable]
    public struct ActionsAllowed
    {
        public bool canMove;
        public bool canJump;
        public bool canAttack;
        public bool canInteract;
        public bool canSwitch;
    }

    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        private float baseMoveSpeed;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        public float _speed { private set; get; }
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private HurtBox _hurtBox;

        [SerializeField] private Material _damageMaterial;
        [HideInInspector] public ObjectMaterialController _materialController;
        [HideInInspector] public UnityEvent charJumpedEvent;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            baseMoveSpeed = MoveSpeed;

            m_ActionsAllowed = new ActionsAllowed
            {
                canAttack = true,
                canMove = true,
                canJump = true,
                canInteract = true,
                canSwitch = true
            };

            try
            {
                _hurtBox = GetComponentInChildren<HurtBox>();
                _hurtBox.hitTaken.AddListener(HurtKnockback);
                _hurtBox.stopAllActions.AddListener(BlockActionsTimed);
            }
            catch
            {
                Debug.Log("Character doesnt have a hurtbox");
            }

            try
            {
                _materialController = GetComponentInChildren<ObjectMaterialController>();
            }
            catch
            {
                Debug.Log("Character doesnt have a material controller");
            }
        }

        private void HurtKnockback(GameObject dmgDealer, float knockbackForce)
        {
            /*
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            StartCoroutine(DamagedMaterialCoroutine());
            */

            Vector3 knockbackDirection;
            if (dmgDealer != null)
            {
                knockbackDirection = transform.position - dmgDealer.transform.position;
                knockbackDirection.y = 0;
                Vector3 lookDirection = -knockbackDirection;
                transform.rotation = Quaternion.LookRotation(lookDirection, transform.up);

                //var direction = dmgDealer.transform.position;
                //direction.y = 0;
                //transform.LookAt(direction);
            }
            else return;
            TakeKnockback(knockbackDirection, 1, knockbackForce);
        }

        IEnumerator DamagedMaterialCoroutine()
        {
            _materialController.AddMaterial(_damageMaterial);
            while (!_hurtBox.IsOpen)
            {
                yield return null;
            }
            _materialController.RemoveMaterial("M_Damaged");
        }

        public void TookHitReaction()
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            StartCoroutine(DamagedMaterialCoroutine());
            GetComponent<AttackManager>().DisableAttack();
            if (!enabled) enabled = true;
            if (!_playerInput.currentActionMap.name.Equals("Player")) _playerInput.SwitchCurrentActionMap("Player");
        }

        private bool blockInput = false;
        ActionsAllowed m_ActionsAllowed;
        [SerializeField] bool debugState;
        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            if (blockInput)
            {
                ResetMoveInput();
                ResetJumpInput();
                ResetAttackInput();
                ResetInteraction();
                ResetSwitch();
            }

            JumpAndGravity();
            GroundedCheck();
            if (knockingback) Knockback();
            else Move();

            if (debugState) Debug.Log(_playerInput.currentActionMap);
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void ResetMoveInput()
        {
            if(!m_ActionsAllowed.canMove) _input.move = Vector2.zero;
        }

        private void ResetAttackInput()
        {
            if (!m_ActionsAllowed.canAttack) _input.attack = false;
        }

        private void ResetJumpInput()
        {
            if (!m_ActionsAllowed.canJump) _input.jump = false;
        }

        private void ResetInteraction()
        {
            if (!m_ActionsAllowed.canInteract) _input.interact = false;
        }
        
        private void ResetSwitch()
        {
            if (!m_ActionsAllowed.canSwitch) 
            {
                _input.changePressed = false;
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        
        public void ChangeMoveSpeed(float newMoveSpd)
        {
            if(newMoveSpd == 0)
            {
                Debug.Log("New speed cannot be zero, canceling speed changing");
                return;
            }

            //baseMoveSpeed = MoveSpeed;
            MoveSpeed = newMoveSpd;
        }

        public void ResetMoveSpeed()
        {
            MoveSpeed = baseMoveSpeed;
        }

        public Vector3 externalMod;
        public float externalSpd;
        private void Move()
        {
            float currentSpeed = _animator.GetBool("LoadedHeavy") ? MoveSpeed / 2 : MoveSpeed;
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : currentSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = (Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward);

            if (externalMod != Vector3.zero)
            {
                _controller.Move(externalMod * (externalSpd * Time.deltaTime));
            }

            // move the player
            _controller.Move(( targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime));
            

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        [SerializeField] private float knockTime;
        [SerializeField] private float knockHeight;
        private float elapsedKnockTime;
        private void Knockback()
        {
            Vector3 finalPos = transform.position + knockbackDirection;

            float verticalMove;
            if (elapsedKnockTime < knockTime)
            {
                elapsedKnockTime += Time.deltaTime;
                verticalMove = CalcParabolaY(transform.position.y, finalPos.y, elapsedKnockTime / knockTime, knockHeight);
            }
            else 
            {
                verticalMove = _verticalVelocity;
                if (Grounded) 
                {
                    StartCoroutine(KnockbackTimeout());
                } 
            } 
            _controller.Move(knockbackDirection.normalized * (knockbackMultiplier * Time.deltaTime) +
                             new Vector3(0f, verticalMove, 0f) * Time.deltaTime);
        }

        System.Collections.IEnumerator KnockbackTimeout()
        {
            yield return new WaitForSeconds(0.1f);
            elapsedKnockTime = 0f;
            knockingback = false;
            _controller.Move(Vector3.zero);
        }

        Vector3 knockbackDirection;
        [SerializeField] bool knockingback = false;
        float knockbackMultiplier = 1f;

        public void TakeKnockback(Vector3 direction, float knockHeight = 1f, float knockForce = 10f, float knockTime = 0.15f)
        {
            if (direction.magnitude == 0) direction = transform.position - transform.forward + (Vector3.up * knockHeight);
            else knockbackDirection = direction;

            Debug.DrawRay(transform.position, direction, Color.green, 10f);

            knockingback = true;
            knockbackMultiplier = knockForce;
            this.knockHeight = knockHeight;
            this.knockTime = knockTime;
        }

        private float CalcParabolaY(float a, float b, float time, float knockbackHeight)
        {
            return (a + ((b - a)) * time + knockbackHeight * (1 - (Mathf.Abs(0.5f - time) / 0.5f) * (Mathf.Abs(0.5f - time) / 0.5f)));
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f && !_animator.GetBool("LoadedHeavy"))
                {
                    charJumpedEvent.Invoke();
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
                _input.jump = false;
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        //private string preJumpState;
        private string pastState;
        public void EnterJumpState()
        {
            //Debug.Log("entering jump");
            pastState = _playerInput.currentActionMap.name;
            _playerInput.SwitchCurrentActionMap("PlayerJumping");
        }
        
        public void ExitJumpState()
        {
            //Debug.Log("exiting jump");
            if (_playerInput.currentActionMap.name.Equals("PlayerJumping")) {
                _playerInput.SwitchCurrentActionMap(pastState);
                pastState = string.Empty;
            }
        }
        
        public void EnterInteractionState()
        {
            if (!_playerInput.currentActionMap.name.Equals("Player")) return;

            if(string.IsNullOrEmpty(pastState)) pastState = _playerInput.currentActionMap.name;
            _playerInput.SwitchCurrentActionMap("PlayerInteracting");
        }

        public void ExitInteractionState()
        {
            if (_playerInput.currentActionMap.name.Equals("PlayerInteracting")) 
                _playerInput.SwitchCurrentActionMap(pastState);
        }

        public CharacterController GetCharController()
        {
            return _controller;
        }

        public void SwitchControl(bool control)
        {
            if (control)
            {
                _playerInput.SwitchCurrentActionMap("PlayerControlled");
                _hurtBox.SetOpenHurtbox(false);
            }
            else 
            {
                _playerInput.SwitchCurrentActionMap("Player");
                _hurtBox.SetOpenHurtbox(true);
            } 
        }

        public void ResetRotation()
        {
            GetComponentInChildren<InteractionController>().ResetHeldObjRotation();
        }

        public void BlockActions(ActionsAllowed actions)
        {
            if (blockInput) return;

            blockInput = true;

            m_ActionsAllowed.canAttack = actions.canAttack;
            m_ActionsAllowed.canInteract = actions.canInteract;
            m_ActionsAllowed.canJump = actions.canJump;
            m_ActionsAllowed.canMove = actions.canMove;
            m_ActionsAllowed.canSwitch = actions.canSwitch;
        }

        public void BlockActionsTimed(ActionsAllowed actions, float duration)
        {
            BlockActions(actions);

            StartCoroutine(StopActionsCoroutine(duration));
        }

        public void RestoreActions()
        {
            blockInput = false;
        }

        IEnumerator StopActionsCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);

            RestoreActions();
        }
    }
}