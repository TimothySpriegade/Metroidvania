using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GroundEnemyScript : MonoBehaviour
{
    #region vars

    #region Movement vars

    [Header("Movement")]
    [SerializeField] private float aggroRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float idleSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallVelocity;
    [SerializeField] private float normalGravity;
    private float distanceToPlayer;
    #endregion

    #region Check vars
    [Header("Checks")]
    [SerializeField] private bool isWalled;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform wallCheckCollider;
    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private LayerMask wallAndGroundCheckLayer;
    private float wallcheckRadius = 0.2f;
    private float groundCheckRadius = 0.2f;
    private bool isFacingRight;
    private GameObject player;
    #endregion

    #region Component vars
    [Header("Components")]
    private Rigidbody2D rb;
    #endregion

    #region idle vars+
    [Header("Idle")]
    [SerializeField] Transform[] idlePoints;
    private int index;
    #endregion

    #endregion

    #region UnityMethods

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.x != 0)
        {
            CheckDirectionToFace(rb.velocity.x > 0);
        }

        WallCheck();
        GroundCheck();
        GetDistToPlayer();
        EnemyAI();
        Jump();
        
    }

    #endregion

    #region Chasing

    private void EnemyAI()
    {
        if (distanceToPlayer < aggroRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    private void ChasePlayer()
    {
        if(transform.position.x < player.transform.position.x)
        {
            // Gegner  auf der linken Seite des Spieler, laufe rechts
            //rb.velocity = new Vector2(moveSpeed, 0);  
            rb.AddForce(moveSpeed * Vector2.right, ForceMode2D.Force);
        }
        else
        {
            // Gegner  auf der rechten Seite des Spieler, laufe links
            //rb.velocity = new Vector2(-moveSpeed, 0);}
            rb.AddForce(moveSpeed * Vector2.left, ForceMode2D.Force);
        }

    }

    private void Idle()
    {
        

        if (Mathf.Abs(transform.position.x - idlePoints[index].position.x) < 0.02f)
        {
            
            index++;
            if (index == idlePoints.Length)
            {
                index = 0;
            }
        }
        var difference = idlePoints[index].position.x - transform.position.x;
        var targetSpeed = Mathf.Sign(difference) * idleSpeed;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);


    }

    #endregion

    #region Checks
    private void WallCheck() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheckCollider.position, wallcheckRadius, wallAndGroundCheckLayer);
        if (colliders.Length>0)
        {
            isWalled = true;
        }
        else
        {
            isWalled = false;
        }
    }
    private void GetDistToPlayer()
    {
        if (!ReferenceEquals(player, null))
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        }

    }
    private void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
        {
            Flip();
        }
    }
    private void GroundCheck()
    {
        if(Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, wallAndGroundCheckLayer)) 
        { 
            isGrounded = true; 
        }
        else 
        { 
        isGrounded = false; 
        }
    }

    #endregion

    #region misc

    private void Flip()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }

    private void Jump()
    {
        if(isWalled)
        {
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
            rb.velocity.Normalize();
        }
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallVelocity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }


    #endregion
}