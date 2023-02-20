using _Core._5_Player.ScriptableObjects;
using DG.Tweening;
using SOEventSystem.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Core._5_Player
{
    [DefaultExecutionOrder(-100)]
    public class PlayerController : MonoBehaviour
    {
        #region Events

        [SerializeField] private VoidEvent onPauseMenuOpen;

        #endregion

        #region Components

        [Header("Data")] [SerializeField] private PlayerControlData data;
        private PlayerMovement movementScript;
        private PlayerCombat combatScript;

        #endregion

        #region Input Vars

        private InputAction horizontalInput;
        private InputAction verticalInput;
        private InputAction jumpInput;
        private InputAction dashInput;
        private InputAction pauseInput;
        private InputAction attackInput;

        private float currentHorizontal;
        private float currentVertical;

        #endregion

        #region Assist

        [Header("Assist")]
        //period when pressing a button when not fulfilling conditions
        //that action will still be performed when conditions fulfilled during time
        [SerializeField]
        [Range(0f, 0.5f)]
        private float inputBufferTime;

        #endregion

        private void Awake()
        {
            movementScript = GetComponent<PlayerMovement>();
            combatScript = GetComponent<PlayerCombat>();
        }

        private void OnEnable()
        {
            EnableAllControls();
        }

        private void OnDisable()
        {
            DisableAllControls();
        }

        private void Update()
        {
            #region Input Handler

            currentHorizontal = horizontalInput.ReadValue<float>();
            currentVertical = verticalInput.ReadValue<float>();

            movementScript.MoveInput = new Vector2(currentHorizontal, currentVertical);

            movementScript.noJumpInput = !jumpInput.inProgress;

            #endregion
        }

        #region Input Callbacks

        private void OnJumpInput(InputAction.CallbackContext context)
        {
            movementScript.LastPressedJumpTime = inputBufferTime;
        }

        private void OnDashInput(InputAction.CallbackContext context)
        {
            movementScript.LastPressedDashTime = inputBufferTime;
        }

        private void OnAttackInput(InputAction.CallbackContext context)
        {
            combatScript.LastPressedAttackTime = inputBufferTime;
        }

        #endregion

        #region Updating Controls

        public void EnableAllControls()
        {
            var controls = data.Controls;
            //Horizontal Controls
            horizontalInput = controls.Movement.Horizontal;
            horizontalInput.Enable();

            //Vertical Controls
            verticalInput = controls.Movement.Vertical;
            verticalInput.Enable();

            //Jump Controls
            jumpInput = controls.Movement.Jump;
            jumpInput.Enable();
            jumpInput.started += OnJumpInput;

            //Dash Controls
            dashInput = controls.Movement.Dash;
            dashInput.Enable();
            dashInput.started += OnDashInput;

            //Attack Controls
            attackInput = controls.Movement.Attack;
            attackInput.Enable();
            attackInput.started += OnAttackInput;

            //pause Controls
            pauseInput = controls.UI.Pause;
            pauseInput.Enable();
            pauseInput.started += ctx => onPauseMenuOpen.Invoke();
        }

        public void DisableAllControls()
        {
            horizontalInput.Disable();
            verticalInput.Disable();
            jumpInput.started -= OnJumpInput;
            jumpInput.Disable();
            dashInput.started -= OnDashInput;
            dashInput.Disable();
            pauseInput.Disable();
            attackInput.Disable();
            attackInput.started -= OnAttackInput;
            pauseInput.started -= _ => onPauseMenuOpen.Invoke();
        }

        public void UpdateControls()
        {
            //Updating all controls
            DisableAllControls();
            EnableAllControls();
        }

        #endregion

        #region Event Handling

        public void ShortControlSuspension(float duration)
        {
            DisableAllControls();
            DOVirtual.DelayedCall(duration, EnableAllControls);
        }

        #endregion
    }
}