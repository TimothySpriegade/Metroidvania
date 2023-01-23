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
        getDistToPlayer();
       
    }
    #endregion

    #region DistanceCheck
    public void getDistToPlayer()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }
    #endregion
}
