using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Vars
    
        #region Components
        private Rigidbody2D rb;
        #endregion

        #region Jump Vars

        [Header("Jump")] 
    
        [SerializeField] private float jumpHeight;
        [SerializeField] private float timeUntilJumpApex;
        [Space(5)]
        //period after falling off a platform, where you can still jump
        [SerializeField] [Range(0f, 0.5f)] private float coyoteTime; 
        //period when pressing a button when not fulfilling conditions
        //that action will still be performed when conditions fulfilled during time
        [SerializeField] [Range(0f, 0.5f)] private float jumpInputBufferTime;
    
    
        private float timeSincePressedJump;
        private float timeSinceGrounded;
        private bool duringJumpCut;
        private bool isJumping;

        #region Wall Jump Vars

        private bool isSliding;
        private bool isWallJumping;

        #endregion
        #endregion
    
        #region Run Vars

        [Header("Movement")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float runAcceleration;
        [SerializeField] private float runDeceleration;
        [SerializeField] private float lerpAmount;
    
        private Vector2 moveInput;
        private static bool isFacingRight;
    
        #endregion

        #region Gravity Vars

        [SerializeField] private float jumpCutGravityMultiplier;
        [SerializeField] private float fallGravityMultiplier;
        [SerializeField] private float maxFallSpeed;

        private float gravityStrength;

        #endregion
    
        #region Dash Vars

        private static bool unlockedDash;

        #endregion
    
        #endregion

        #region Unity Methods
    
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            //calculating gravity strength using the formula 'gravity = 2 * jumpHeight / timeToJumpApex^2'
            gravityStrength = (-(2 * jumpHeight) / (timeUntilJumpApex * timeUntilJumpApex)) / -30f;
        }

        #region Update Methods
    
        private void Update()
        {
            #region Timers

            timeSinceGrounded += Time.deltaTime;
            timeSincePressedJump += Time.deltaTime;
        

            #endregion
        
            #region INPUT HANDLER
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        
            if (moveInput.x != 0)
                CheckDirectionToFace(moveInput.x > 0);
        
            //TODO change to Unity input manager
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
            {
                OnJumpInput();
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
            {
                OnJumpUpInput();
            }
        
            #endregion

            #region Jump Checks
        
        

            #endregion
        
            #region Gravity Handling

            //No gravity when sliding (sliding is a velocity)
            //TODO explanation
            if (isSliding)
            {
                SetGravityScale(0);
            }
            //Higher gravity if jump is released during jump
            else if (duringJumpCut)
            {
                //multiplies the idle gravity strength by the jump cut-gravity multiplier
                SetGravityScale(gravityStrength * jumpCutGravityMultiplier);
                //caps fall-speed at max fall speed
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxFallSpeed));
            } 
            //When falling
            else if (rb.velocity.y < 0)
            {
                //multiplies the idle gravity strength by the fall-gravity multiplier
                SetGravityScale(gravityStrength * fallGravityMultiplier);
                //caps fall-speed at max fall speed
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxFallSpeed));
            }
            else
            {
                SetGravityScale(gravityStrength);
            }
        

            #endregion
        }
    
        private void FixedUpdate()
        {
            Run();
        }
        #endregion
        #endregion
    
        #region Run

        private void Run()
        {
            //moveInput * moveSpeed = desired speed. (moveInput at max would be top speed / moveInput at 0 would be standing)
            var desiredSpeed = moveInput.x * maxSpeed;
            //Lerping that value for smoothing
            desiredSpeed = Mathf.Lerp(rb.velocity.x, desiredSpeed, lerpAmount);

            //determines if the player is acceleration or decelerating. When Airborne the accelRate is multiplied by airAcceleration/Deceleration
            float accelRate;
        
            if (timeSinceGrounded > 0)
                accelRate = (Mathf.Abs(desiredSpeed) > 0.01f) ? runAcceleration : runDeceleration;
            else
                accelRate = (Mathf.Abs(desiredSpeed) > 0.01f) ? runAcceleration : runDeceleration;

            //calculating accelerating using formula: 1 / Time.fixedDeltaTime * acceleration / max Speed
            accelRate = ((1 / Time.fixedDeltaTime) * accelRate) / maxSpeed;
        
            //difference current and desired speed
            var speedDif = desiredSpeed - rb.velocity.x;
        
            //multiplies speedDif with the calculated acceleration rate
            var movement = speedDif * accelRate;
        
            //applies force
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        private void Turn()
        {
            //stores scale and flips the player along the x axis, 
            var scale = transform.localScale; 
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }
    

        #endregion

        #region Jump

        private void Jump()
        {
            timeSinceGrounded = 0;
            timeSincePressedJump = 0;
        
            var jumpForce = Mathf.Abs(gravityStrength) * timeUntilJumpApex;
            //adds falling velocity to jump so you dont lose height when relying on coyote time
            if (rb.velocity.y < 0)
                jumpForce -= rb.velocity.y;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void WallJump()
        {
        
        }
    
        private void OnJumpInput()
        {
            timeSincePressedJump = jumpInputBufferTime;
        }

        private void OnJumpUpInput()
        {
            if (isJumping && rb.velocity.y > 0)
            {
                duringJumpCut = true;
            }
        }

        #endregion

        #region Gravity

        private void SetGravityScale(float scale)
        {
            rb.gravityScale = scale;
        }
    

        #endregion
    
        #region Check Methods

        private void CheckDirectionToFace(bool isMovingRight)
        {
            if (isMovingRight != isFacingRight)
            {
                Turn();
            }
        }

        private bool CanJump()
        {
            return false;
        }

        #endregion
    }
}
