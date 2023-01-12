using Player.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Components

        [Header("Data")] [SerializeField] private PlayerControlData data;
        private PlayerMovement movementScript;

        #endregion
    
        #region Input Vars

        private InputAction horizontalInput;
        private InputAction verticalInput;
        private InputAction jumpInput;
        private InputAction dashInput;

        private float currentHorizontal;
        private float currentVertical;

        #endregion
    
        #region Assist
    
        [Header("Assist")]
        //period when pressing a button when not fulfilling conditions
        //that action will still be performed when conditions fulfilled during time
        [SerializeField] [Range(0f, 0.5f)] private float inputBufferTime;
    
        #endregion

        private void Start()
        {
            movementScript = GetComponent<PlayerMovement>();
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

        #endregion
        
        #region Updating Controls

        private void EnableAllControls()
        {
            var controls = data.Controls;
            //Horizontal Controls
            horizontalInput = controls.Player.Horizontal;
            horizontalInput.Enable();

            //Vertical Controls
            verticalInput = controls.Player.Vertical;
            verticalInput.Enable();

            //Jump Controls
            jumpInput = controls.Player.Jump;
            jumpInput.Enable();
            jumpInput.started += OnJumpInput;

            //Dash Controls
            dashInput = controls.Player.Dash;
            dashInput.Enable();
            dashInput.started += OnDashInput;
        }

        private void DisableAllControls()
        {
            horizontalInput.Disable();
            verticalInput.Disable();
            jumpInput.started -= OnJumpInput;
            jumpInput.Disable();
            dashInput.started -= OnDashInput;
            dashInput.Disable();
            
        }

        public void UpdateControls()
        {
            //Updating all controls
            DisableAllControls();
            EnableAllControls();
        }

        #endregion
    }
}
