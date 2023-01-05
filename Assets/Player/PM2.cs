using UnityEngine;

public class PM2 : MonoBehaviour
{
    #region Components
    private Rigidbody2D rb;
    #endregion

    #region Jump Vars

    [Header("Jump")] 
    
    [SerializeField] private float jumpForce;
    [Space(5)]
    //period after falling off a platform, where you can still jump
    [SerializeField] [Range(0f, 0.5f)] private float coyoteTime; 
    //period when pressing a button when not fulfilling conditions
    //that action will still be performed when conditions fulfilled during time
    [SerializeField] [Range(0f, 0.5f)] private float jumpInputBufferTime;
    
    private float timeSincePressedJump;
    private float timeSinceGrounded;

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

    #region Dash Vars

    private static bool unlockedDash;

    #endregion

    private float test;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        
        
        
        #endregion
    }
    
    private void FixedUpdate()
    {
        Run();
    }
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
