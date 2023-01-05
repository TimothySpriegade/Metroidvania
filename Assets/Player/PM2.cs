using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PM2 : MonoBehaviour
{
    #region Components
    private Rigidbody2D rb;
    #endregion

    #region Jump Vars
    
    [Header("Jump")]
    //period after falling off a platform, where you can still jump
    [SerializeField] [Range(0.01f, 0.5f)] private float coyoteTime; 
    //period when pressing a button when not fulfilling conditions
    //that action will still be performed when conditions fulfilled during time
    [SerializeField] [Range(0.01f, 0.5f)] private float jumpInputBufferTime;

    private float timeSinceGrounded;

    #endregion
    
    #region Run Vars

    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float runAcceleration;
    [SerializeField] private float runDeceleration;
    [SerializeField] [Range(0f, 1f)] private float airAcceleration;
    [SerializeField] [Range(0f, 1f)] private float airDeceleration;

    [SerializeField] private float frictionAmount;

    private Vector2 moveInput;
    private static bool isFacingRight;
    
    #endregion

    #region Dash Vars

    private static bool unlockedDash;

    #endregion
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    #region Update Methods
    
    private void Update()
    {
        #region Timers
        
        

        #endregion
        
        #region INPUT HANDLER
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        if (moveInput.x != 0)
            CheckDirectionToFace(moveInput.x > 0);
        
        #endregion
    }
    
    private void FixedUpdate()
    {
        Run();
        Friction();
    }
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
            accelRate = (Mathf.Abs(desiredSpeed) > 0.01f) ? runAcceleration * airAcceleration : runDeceleration * airDeceleration;

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

    private void Friction()
    {
        if (timeSinceGrounded > 0 && moveInput.x == 0)
        {
            //checks if velocity or friction is higher
            var friction = Mathf.Min(Mathf.Abs(rb.velocity.x), frictionAmount);
            //
            friction *= Mathf.Sign(rb.velocity.x);
            
            rb.AddForce(Vector2.right * -friction, ForceMode2D.Impulse);
        }
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
