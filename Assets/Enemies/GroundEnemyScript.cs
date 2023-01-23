using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyScript : MonoBehaviour
{
    #region vars

    [SerializeField] private Transform player;
    [SerializeField] private float aggroRange;
    [SerializeField] private float moveSpeed;
    private float distanceToPlayer;
    private bool isFacingRight;
    private Rigidbody2D rb;
    #endregion

    #region UnityMethods
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetDistToPlayer();
        ChasePlayerAI();
        IsFacingRight();
        Flip();
      
    }
    #endregion

    #region DistanceCheck&Chasing
    private void GetDistToPlayer()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }

    private void ChasePlayerAI()
    {
        if (distanceToPlayer < aggroRange)
        {
            ChasePlayer();
        }
        else
        {
            StopChasing();
        }
    }

    private void ChasePlayer()
    {
        if(transform.position.x < player.position.x)
        {
            // Gegner  auf der linken Seite des Spieler, laufe rechts
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            // Gegner  auf der rechten Seite des Spieler, laufe links
            rb.velocity = new Vector2(-moveSpeed, 0);
        }
        
    }

    private void StopChasing()
    {
        rb. velocity = Vector2.zero;
    }

    #endregion

    #region misc

    private void IsFacingRight()
    {
        var scalex = transform.localScale.x;
        if (scalex == -1) 
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }
    private void Turn()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void Flip()
    {
        if (rb.velocity.x == -moveSpeed && isFacingRight) 
        {
            Turn();
        }
        if (rb.velocity.x == moveSpeed && !isFacingRight)
        {
            Turn();
        }
    }
    #endregion
}
