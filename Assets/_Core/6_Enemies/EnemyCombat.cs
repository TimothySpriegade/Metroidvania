using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    #region movementvars
    [Header("Movement")]
    [SerializeField] private float knockbackStrengthX;
    [SerializeField] private float knockbackStrengthY;
    #endregion

    #region checkvars
    [Header ("Checks")]
    [SerializeField] private bool isFacingRight;
    private Rigidbody2D rb;
    #endregion

    #region components
    [Header("Components")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform enemy;
    #endregion

    #region CombatVars
    private float enemyHealth;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = enemyData.enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        IsFacingRight();
        Death();
    }
    private void Death()
    {
        if (enemyHealth <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        Debug.Log(enemyHealth);
    }

    public void TakeKnockback()
    {
        if (!isFacingRight) 
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(knockbackStrengthX * 1000 * Vector2.right, ForceMode2D.Force);
            rb.AddForce(knockbackStrengthY* 1000 * Vector2.up, ForceMode2D.Force);

            Debug.Log("Knockbacked");
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(knockbackStrengthX * 1000 * Vector2.left, ForceMode2D.Force);
            rb.AddForce(knockbackStrengthY * 1000 * Vector2.up, ForceMode2D.Force);

            Debug.Log("Knockbacked");
        }

    }

    public void IsFacingRight()
    {
        isFacingRight = enemy.localScale.x <= 0;
    }


}
