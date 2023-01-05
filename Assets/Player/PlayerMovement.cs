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
        private float timeSinceWallTouch;

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

        [SerializeField] private float fallGravityMultiplier;
        [SerializeField] private float maxFallSpeed;

        private float gravityStrength;

        #endregion
    
        #region Dash Vars

        private static bool unlockedDash;

        #endregion
        
        #region Check Vars
        //Set all of these up in the inspector
        [Header("Checks")] 
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
        [Space(5)]
        [SerializeField] private Transform frontWallCheckPoint;
        [SerializeField] private Transform backWallCheckPoint;
        [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1f);
        #endregion
        
        #region Layers and Tags
        [Header("Layers & Tags")]
        [SerializeField] private LayerMask groundLayer;
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
            gravityStrength = -(2 * jumpHeight) / (timeUntilJumpApex * timeUntilJumpApex) / -30f;
        }

        #region Update Methods
    
        private void Update()
        {
            #region Timers

            timeSinceGrounded += Time.deltaTime;
            timeSincePressedJump += Time.deltaTime;
            timeSinceWallTouch += Time.deltaTime;
        

            #endregion
        
            #region Collision Checks
            //Ground Check
                if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
                {
                    timeSinceGrounded = coyoteTime;
                }		

                //Wall check
                if ((Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer))
                    || (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer)))
                {
                    timeSinceWallTouch = coyoteTime;
                }
                #endregion
            
            #region INPUT HANDLER
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        
            if (moveInput.x != 0)
                CheckDirectionToFace(moveInput.x > 0);

            #endregion

            #region Gravity Handling

            //No gravity when sliding (sliding is a velocity)
            if (isSliding)
            {
                SetGravityScale(0);
            }
            //When falling
            else if (rb.velocity.y < 0)
            {
                //multiplies the idle gravity strength by the fall-gravity multiplier
                SetGravityScale(gravityStrength * fallGravityMultiplier);
                //caps fall-speed at max fall speed
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
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
        #endregion
    }
}
