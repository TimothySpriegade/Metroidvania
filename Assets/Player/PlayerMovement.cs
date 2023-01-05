using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Components
        private Rigidbody2D rb;
        public LayerMask ground;
        #endregion

        #region Jump Vars
        [Header("Jump")] 
        [SerializeField] private Transform feetPos;
        [SerializeField] private Vector2 feetSize;
        [SerializeField] private float jumpMaxTime;
        [SerializeField] private float jumpForce;
        [SerializeField] private float coyoteTime;
        
        private bool isGrounded;
        private bool isJumping;
        private float timeSinceGrounded;
        private float jumpTimeLeft;
        #endregion
        
        #region Run Vars
        [Header("Run")] 
        [SerializeField] private float movementSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float velPower;
        
        [SerializeField] private float frictionAmount;
        
        private float moveInputX;
        private float moveInputY;
        
        #endregion
        
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            isGrounded = false;
        }


        #region Jump
        void Jump()
        {
            //When button pressed and grounded -> Jumps
            if (Input.GetKeyDown("space") && (isGrounded || timeSinceGrounded >= 0))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                jumpTimeLeft = jumpMaxTime;
            }
        
            //When holding key and key was not let go -> keep going up
            if (Input.GetKey("space") && jumpTimeLeft > 0 && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeLeft -= Time.deltaTime;
            }

            //When key is released = dont allow any further jumping during that airtime
            if (Input.GetKeyUp("space") || jumpTimeLeft <= 0)
            {
                isJumping = false;
            }
        }
        #endregion

        #region Run
        
        void Movement()
        {
            moveInputX = Input.GetAxisRaw("Horizontal");
            //Our desired speed
            var targetSpeed = moveInputX * movementSpeed;
            //Difference of desired speed with current speed
            var speedDif = targetSpeed - rb.velocity.x;
            //Checks if you are accelerating or decelerating. If wants to move: Acceleration - Else: Deceleration
            var accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

            var movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rb.AddForce(movement * Vector2.right);


            //transform.position = new Vector3((_playerPos.x + _moveInput * movementSpeed * Time.deltaTime), _playerPos.y);
        }

        void Friction()
        {
            if (!isGrounded || !(Mathf.Abs(moveInputX) < 0.01f)) return;
        
            //Take *either* the current speed *or* the given friction amount, depending on which one is smaller
            var amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));

            //Change the sign of the amount
            amount *= Mathf.Sign(rb.velocity.x);

            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        void TurnAround()
        {
            if (moveInputX > 0)
            {
                transform.eulerAngles = Vector3.up * 0;
            }
            else if (moveInputX < 0)
            {
                transform.eulerAngles = Vector3.up * 180;
            }
        }
        
        #endregion

        private void Update()
        {
        
            #region PlayerGroundedCheck

            if (Physics2D.OverlapBox(feetPos.position, feetSize, 0, ground))
            {
                isGrounded = true;
                timeSinceGrounded = coyoteTime;
            }
            else
            {
                isGrounded = false;
                timeSinceGrounded -= Time.deltaTime;
            }

            #endregion
       
            Jump();
            TurnAround();
            Friction();

        }
        void FixedUpdate()
        {
            Movement();
        }
    }
}