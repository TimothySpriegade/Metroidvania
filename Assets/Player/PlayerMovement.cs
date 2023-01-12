using System;
using System.Collections;
using SOEventSystem.Events;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Vars

        #region Events


        #endregion
        
        #region Components
        
        private Rigidbody2D rb;

        #endregion

        #region Jump Vars

        [Header("Jump")] 
        [SerializeField] private float jumpHeight;
        [SerializeField] private float timeUntilJumpApex;

        [Space(5)]
        [Tooltip("Threshold how close to the apex the player has to be to activate the gravity multiplier")]
        [SerializeField]
        private float jumpHangThreshold;

        [SerializeField] [Range(0f, 1f)] private float jumpHangGravityMultiplier;

        public float LastPressedJumpTime { get; set; }
        private float lastGroundedTime;
        private bool duringJumpCut;
        private bool isJumping;
        public bool noJumpInput;

        #region Wall Jump Vars

        [Header("Wall Jump")] 
        [SerializeField] private float wallJumpLerp;

        //time until you can wall jump again
        [SerializeField] private float wallJumpTime;
        [SerializeField] private Vector2 wallJumpForce;

        private bool isSliding;
        private bool isWallJumping;
        private float lastWallJumped;
        private float lastLeftWallTouchTime;
        private float lastRightWallTouchTime;

        #endregion

        #endregion

        #region Run Vars

        [Header("Movement")] 
        [SerializeField] private float maxSpeed;
        [SerializeField] private float runAcceleration;
        [SerializeField] private float runDeceleration;

        public Vector2 MoveInput { get; set; }
        private static bool isFacingRight;

        #endregion

        #region Dash Vars

        [Header("Dash")] 
        [SerializeField] private float dashForceMultiplier;
        [SerializeField] private float dashLength;
        [SerializeField] private float dashCooldown;
        [Tooltip("Tiny amount the dash freezes the game to make the Dash feel more impactful")]
        [SerializeField] private float dashSleepTime;
        [Tooltip("Deadzone until the game recognizes your input to dash into that direction (0, 0.3) wont make you dash upwards")]
        [SerializeField] private float controllerInputThreshold;

        public float LastPressedDashTime { get; set; }
        private float lastDashed;

        private Vector2 dashDirection;

        private bool dashRefreshed;
        private bool isDashAttacking;
        private bool isDashing;
        private static bool unlockedDash;

        #endregion

        #region Gravity Vars

        [Header("Gravity")] 
        [SerializeField] private float fallGravityMultiplier;
        [SerializeField] private float jumpCutGravityMultiplier;
        [SerializeField] private float maxFallSpeed;

        [Space(5)]
        //Fall speed will be capped to this when sliding. Should be lower than maxFallSpeed
        [SerializeField]
        private float slideSpeed;

        private float gravityStrength;

        #endregion

        #region Assist Vars

        [Header("Assists")]
        //period after falling off a platform, where you can still jump
        [SerializeField]
        [Range(0f, 0.5f)]
        private float coyoteTime;

        #endregion
        
        #region Check Vars

        //Set all of these up in the inspector
        [Header("Checks")] 
        [SerializeField] private Transform groundCheckPoint;

        [SerializeField] private Vector2 groundCheckSize;
        [Space(5)] [SerializeField] private Transform frontWallCheckPoint;

        [SerializeField] private Vector2 wallCheckSize;

        #endregion

        #region Layers and Tags

        [Header("Layers & Tags")] 
        [SerializeField] private LayerMask groundLayer;

        #endregion

        #endregion

        #region Unity Methods

        #region Start methods

        private void Awake()
        {
            
            rb = GetComponent<Rigidbody2D>();
            var localScale = transform.localScale;
            groundCheckSize *= localScale;
            wallCheckSize *= localScale;
            unlockedDash = true;
        }

        private void Start()
        {
            //calculating gravity strength using the formula 'gravity = 2 * jumpHeight / timeToJumpApex^2'
            gravityStrength = -(2 * jumpHeight) / (timeUntilJumpApex * timeUntilJumpApex) / -30f;
            isFacingRight = true;
        }

        #endregion

        private void Update()
        {
            #region Timers

            lastGroundedTime -= Time.deltaTime;
            LastPressedJumpTime -= Time.deltaTime;
            LastPressedDashTime -= Time.deltaTime;
            lastLeftWallTouchTime -= Time.deltaTime;
            lastRightWallTouchTime -= Time.deltaTime;
            lastWallJumped -= Time.deltaTime;
            lastDashed -= Time.deltaTime;

            #endregion

            #region Collision Checks

            if (!isJumping)
            {
                //Ground Check
                if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
                {
                    lastGroundedTime = coyoteTime;
                }

                //Right-Wall check
                if (Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && isFacingRight)
                {
                    lastRightWallTouchTime = coyoteTime;
                }

                //Left-Wall check
                if (Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !isFacingRight)
                {
                    lastLeftWallTouchTime = coyoteTime;
                }
            }

            #endregion

            #region Input Handler

            if (MoveInput.x != 0)
                CheckDirectionToFace(MoveInput.x > 0);

            if (noJumpInput)
            {
                if (CanJumpCut()) duringJumpCut = true;
            }

            #endregion

            #region Jump Checks

            if (!isDashing)
            {
                if (IsNotJumping())
                {
                    isJumping = false;
                }

                //time after having performed a wall jump surpasses chosen jump time
                if (lastWallJumped < -wallJumpTime)
                {
                    isWallJumping = false;
                }

                if (lastGroundedTime > 0 && !isJumping)
                {
                    duringJumpCut = false;
                }

                //Jump
                if (CanJump() && LastPressedJumpTime > 0)
                {
                    isJumping = true;
                    isWallJumping = false;
                    Jump();
                }

                //Wall jump
                if (CanWallJump() && LastPressedJumpTime > 0)
                {
                    isWallJumping = true;
                    isJumping = false;
                    duringJumpCut = false;

                    var awayFromWallDirection = (lastRightWallTouchTime > 0) ? -1 : 1;

                    WallJump(awayFromWallDirection);
                }
            }

            #endregion

            #region Dash Checks

            if (lastGroundedTime > 0)
            {
                dashRefreshed = true;
            }

            if (CanDash() && LastPressedDashTime > 0)
            {
                isDashing = true;
                isJumping = false;
                isWallJumping = false;
                duringJumpCut = false;


                //Sleep to add weight behind the dash
                Sleep(dashSleepTime);
                
                //Normalizes input to ensure that controller wont dash unwanted angles
                dashDirection = NormalizeMoveInput(MoveInput);

                //When standing still or dashing down => Dashing forward
                if (dashDirection == Vector2.down && lastGroundedTime > 0) 
                    dashDirection = isFacingRight ? Vector2.right : Vector2.left;
                else if (dashDirection == Vector2.zero)
                    dashDirection = isFacingRight ? Vector2.right : Vector2.left;

                StartCoroutine(Dash(dashDirection));
            }

            #endregion

            #region Slide Checks

            isSliding = CanSlide();

            if (isSliding)
            {
                duringJumpCut = false;
            }

            #endregion

            #region Gravity Handler

            if (!isDashing)
            {
                //if jump is released during jump = gravity increase
                if (duringJumpCut)
                {
                    //multiplies the idle gravity strength by the jump cut-gravity multiplier
                    SetGravityScale(gravityStrength * jumpCutGravityMultiplier);
                    //caps fall-speed at max fall speed
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
                }
                //Jump Hang
                else if ((isJumping || isWallJumping) && Mathf.Abs(rb.velocity.y) < jumpHangThreshold)
                {
                    SetGravityScale(gravityStrength * jumpHangGravityMultiplier);
                }
                //When falling
                else if (rb.velocity.y < 0)
                {
                    //multiplies the idle gravity strength by the fall-gravity multiplier
                    SetGravityScale(gravityStrength * fallGravityMultiplier);
                    //caps fall-speed at max fall speed or at sliding speed
                    rb.velocity = isSliding
                        ? new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -slideSpeed))
                        : new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
                }
                else
                {
                    SetGravityScale(gravityStrength);
                }
            }

            #endregion
        }

        private void FixedUpdate()
        {
            if (!isDashing)
            {
                //Run
                if (isWallJumping)
                    Run(wallJumpLerp);
                else
                    Run(1);
            }
        }

        #endregion

        #region Run

        private void Run(float lerp)
        {
            //moveInput * moveSpeed = desired speed. (moveInput at max would be top speed / moveInput at 0 would be standing)
            var desiredSpeed = MoveInput.x * maxSpeed;
            desiredSpeed = Mathf.Lerp(rb.velocity.x, desiredSpeed, lerp);


            //determines if the player is acceleration or decelerating. When Airborne the accelRate is multiplied by airAcceleration/Deceleration
            float accelRate;

            if (lastGroundedTime > 0)
                accelRate = (Mathf.Abs(desiredSpeed) > 0.01f) ? runAcceleration : runDeceleration;
            else
                accelRate = (Mathf.Abs(desiredSpeed) > 0.01f) ? runAcceleration : runDeceleration;

            //calculating accelerating using formula: 1 / Time.fixedDeltaTime * acceleration / max Speed
            accelRate = ((1 / Time.fixedDeltaTime) * accelRate) / maxSpeed;

            //difference current and desired speed
            var speedDiff = desiredSpeed - rb.velocity.x;

            //multiplies speedDif with the calculated acceleration rate
            var movement = speedDiff * accelRate;

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
            LastPressedJumpTime = 0;
            lastGroundedTime = 0;

            //We increase the force applied if we are falling
            //This means we'll always feel like we jump the same amount 
            var jumpForce = timeUntilJumpApex * gravityStrength * 30;

            if (rb.velocity.y < 0) jumpForce -= rb.velocity.y;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void WallJump(int direction)
        {
            //Ensures no extra jumps can be made
            LastPressedJumpTime = 0;
            lastGroundedTime = 0;
            lastRightWallTouchTime = 0;
            lastLeftWallTouchTime = 0;
            lastWallJumped = 0;

            var jumpForce = wallJumpForce;

            //Left or Right force away from the wall
            jumpForce.x *= direction;

            if (rb.velocity.y < 0) jumpForce.y -= rb.velocity.y;

            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }

        #endregion

        #region Sleep Methods

        private void Sleep(float duration)
        {
            //Starts Coroutine which pauses game for short time
            //Coroutines can happen in multiple frames.
            //If you have a loop that counts 5000 times in Update() then those 5000 times would happen in 1 frame.
            //With coroutines you can split that into multiple frames
            StartCoroutine(nameof(PerformSleep), duration);
        }

        private IEnumerator PerformSleep(float duration)
        {
            //timescale = speed at which the game moves. 0 = game doesnt move / 1 = normal speed
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
        }

        #endregion

        #region Dash

        private Vector2 NormalizeMoveInput(Vector2 moveInput)
        {
            if (Mathf.Abs(moveInput.x) < controllerInputThreshold) moveInput.x = 0;
            if (Mathf.Abs(moveInput.y) < controllerInputThreshold) moveInput.y = 0;

            moveInput.x = moveInput.x != 0 ? Mathf.Sign(moveInput.x) : 0;
            moveInput.y = moveInput.y != 0 ? Mathf.Sign(moveInput.y) : 0;

            return moveInput;
        }

        private IEnumerator Dash(Vector2 direction)
        {
            LastPressedDashTime = 0;

            var startTime = Time.time;
            
            //Dashes diagonal
            var lengthMultiplier = direction.x != 0 && direction.y != 0 ? 0.75f : 1f;
            
            SetGravityScale(0);
            
            //Keeps player velocity at Dash Speed
            while (Time.time - startTime <= dashLength * lengthMultiplier)
            {
                rb.velocity = direction.normalized * (maxSpeed * dashForceMultiplier);
                //Pauses the loop until the next frame, creating something of a Update loop. 
                yield return null;
            }

            //Dash over
            lastDashed = dashCooldown;
            isDashing = false;
            duringJumpCut = true;
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
            if (isMovingRight != isFacingRight && !isDashing)
            {
                Turn();
            }
        }

        private bool CanJump()
        {
            return lastGroundedTime > 0 && !isJumping;
        }

        private bool CanWallJump()
        {
            return !isWallJumping && LastPressedJumpTime > 0 && lastGroundedTime <= 0 &&
                   (lastRightWallTouchTime > 0 || lastLeftWallTouchTime > 0);
        }

        private bool CanJumpCut()
        {
            return (isJumping || isWallJumping) && rb.velocity.y > 0;
        }

        private bool CanSlide()
        {
            var canSlide = (lastLeftWallTouchTime > 0 && MoveInput.x < 0) ||
                           (lastRightWallTouchTime > 0 && MoveInput.x > 0);
            canSlide = canSlide && rb.velocity.y <= 0 && lastGroundedTime <= 0;
            return canSlide;
        }

        private bool IsNotJumping()
        {
            return (isWallJumping || isJumping) && rb.velocity.y <= 0 || lastGroundedTime > 0;
        }

        private bool CanDash()
        {
            return unlockedDash && !isDashing && lastDashed < 0 && dashRefreshed;
        }

        #endregion
    }
}