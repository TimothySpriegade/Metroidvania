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
using _Core._6_Characters.Enemies;
using DG.Tweening;

public class PlayerCombatHandler : MonoBehaviour
{
    #region vars

    #region DamageHandeling Vars
    [Header("Damage Handeling")]
    [SerializeField] private PlayerCombat playerCombatScript;
    [SerializeField] private Vector2 knockbackStrength;
    private float invinceTime;

    #endregion

    #region Health Vars
    [Header("Health")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHearts;
    [SerializeField] private Sprite emptyHearts;

    #endregion

    #region Data
    [Header("Data")]
    [SerializeField] private PlayerCombatData playerData;
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

        HealthSpriteFinder();
    }

    public void Update()
    {
        invinceTime -= Time.deltaTime;
        HealthSpriteUpdater();
    }
    #endregion

    #region healthMethods
    private void HealthSpriteUpdater()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            playerData.currentHealth = playerData.currentHealth > playerData.maxHealth ? playerData.maxHealth : playerData.currentHealth;

            hearts[i].sprite = i < playerData.currentHealth ? fullHearts : emptyHearts;

            hearts[i].enabled = i < playerData.maxHealth;
        }
    }

    private void HealthSpriteFinder()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = GameObject.Find($"Heart ({i})").GetComponent<Image>();
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (invinceTime <= 0)
            {
                invinceTime = 0.5f;
                var enemyDamage = collision.gameObject.GetComponentInParent<EnemyCombat>().enemyData.damage;
                TakeDamage(enemyDamage);
            }
        }
    }

    private void TakeDamage(int amount)
    {
        playerData.currentHealth -= amount;
        TakeKnockBack();
     /* if (currentHealth <= 0)
         {
             GameObject.Destroy(gameObject);
         } */
    }

    public void TakeKnockBack()
    {
        player.IgnoreRun = true;

        var direction = player.IsFacingRight ? Vector2.left : Vector2.right;
        rb.velocity = Vector3.zero;
        rb.AddForce(knockbackStrength.x * direction, ForceMode2D.Impulse);
        rb.AddForce(knockbackStrength.y * Vector2.up, ForceMode2D.Impulse);
        DOVirtual.DelayedCall(0.5f, () => {
            player.IgnoreRun = false;
        });
        
    }
}
