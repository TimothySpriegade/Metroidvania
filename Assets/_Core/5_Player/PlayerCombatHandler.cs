using _Core._5_Player.ScriptableObjects;
using _Core._6_Characters.Enemies.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;
using _Core._5_Player;
using System.Net.Security;

public class PlayerCombatHandler : MonoBehaviour
{
    #region vars

    #region DamageHandeling Vars
    [Header("Damage Handeling")]
    [SerializeField] private PlayerCombat playerCombatScript;
    [SerializeField] private Vector2 knockbackStrength;
    private float lastHitTime;
    private float invisTime;

    #endregion

    #region Health Vars
    [Header("Health")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHearts;
    [SerializeField] private Sprite emptyHearts;

    private int currentHealth;
    private int numbOfHeartContainers;
    #endregion

    #region Data
    [Header("Data")]
    [SerializeField] private PlayerCombatData playerData;
    private EnemyData enemyData;
    private PlayerMovement player;
    #endregion

    #region Components
    private Rigidbody2D rb;
    #endregion

    #endregion

    #region UnityMethods
    public void Awake()
    {
        player = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = (int)playerData.maxHealth;
        numbOfHeartContainers = (int)playerData.maxHealth;
        HealthSpriteFinder();
    }

    public void Update()
    {
        lastHitTime -= Time.deltaTime;
        invisTime -= Time.deltaTime;
        HealthSpriteUpdater();
    }
    #endregion

    #region healthMethods
    public void HealthSpriteUpdater()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            currentHealth = currentHealth > numbOfHeartContainers ? numbOfHeartContainers : currentHealth;

            hearts[i].sprite = i < currentHealth ? fullHearts : emptyHearts;

            hearts[i].enabled = i < numbOfHeartContainers;
        }
    }

    public void HealthSpriteFinder()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = GameObject.Find("Heart " + "(" + i + ")").GetComponent<Image>();
        }
        Debug.Log("Health loaded");
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Enemy") && playerCombatScript.LastPressedAttackTime <= -0.4f)
        {
            lastHitTime = 0;
            if (invisTime <= -0.5f)
            {
                invisTime = 0;
                TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        TakeKnockBack();
    }

    public void TakeKnockBack()
    {
        var direction = player.IsFacingRight ? Vector2.left : Vector2.right;
     
        rb.velocity = Vector3.zero;
        rb.AddForce(knockbackStrength.x * direction, ForceMode2D.Impulse);
        rb.AddForce(knockbackStrength.y * Vector2.up, ForceMode2D.Impulse);
        
    }
}
