using SOEventSystem.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    #region vars

    #region playercombatvars

    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;
    public float LastPressedAttackTime { get; set; }

    #endregion

    #region events

    [Header("Events")]
    [SerializeField] private FloatEvent onDamageGiven;

    #endregion



    #endregion

    #region UnityMethods

    private void Update()
    {
        LastPressedAttackTime -= Time.deltaTime;

        if (LastPressedAttackTime > 0) //TODO Movementbedingungen hinzufügen
        {
            Attack();
        }
    }

    #endregion

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies) 
        {
            onDamageGiven.Invoke(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
