using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundEnemyScript : MonoBehaviour
{
    #region vars

    #region Movement vars

    [Header("Movement")]
    [SerializeField] private float aggroRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float idleSpeed;
    private float distanceToPlayer;
    #endregion

    #region Check vars
    [Header("Checks")]
    [SerializeField] private bool isWalled;
    [SerializeField] private float wallcheckRadius;
    [SerializeField] private Transform wallCheckCollider;
    [SerializeField] private LayerMask wallCheckLayer;
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
        WallCheck();
        GetDistToPlayer();
        EnemyAI();
        //Debug.Log(index);
    }
    #endregion

    #region DistanceCheck&Chasing
    private void GetDistToPlayer()
    {
        if(!ReferenceEquals(player, null))
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        }
       
    }

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
            rb.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);   
        }
        else
        {
            // Gegner  auf der rechten Seite des Spieler, laufe links
            rb.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
        }
        
    }

    private void Idle()
    {
        if (Mathf.Abs(transform.position.x - idlePoints[index].position.x) < 1f)
        {
            Debug.Log("!!!");
            index++;
            if (index == idlePoints.Length)
            {
                index = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, idlePoints[index].position, idleSpeed * Time.deltaTime);

    }

    #endregion

    #region Checks
    private void WallCheck() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheckCollider.position, wallcheckRadius, wallCheckLayer);
        if (colliders.Length>0)
        {
            isWalled = true;
        }
        else
        {
            isWalled = false;
        }
    }

    #endregion

    #region misc
    #endregion
}
