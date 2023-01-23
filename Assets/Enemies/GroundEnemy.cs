using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    #region vars

    [SerializeField] private Transform player;
    [SerializeField] private float aggroRange;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
   
    }

   
}
