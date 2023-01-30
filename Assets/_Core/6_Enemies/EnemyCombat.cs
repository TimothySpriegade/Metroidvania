using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    #region components
    [SerializeField] private EnemyData enemyData;
    #endregion

    #region CombatVars
    private float enemyHealth;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = enemyData.enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Test()
    {
        Debug.Log("Test");
    }

}
