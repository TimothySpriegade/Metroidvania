using _Core._5_Player;
using _Core._5_Player.ScriptableObjects;
using _Core._6_Characters.Enemies;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Core._9_UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        #region vars

        #region DamageHandling Vars

        [Header("Damage Handling")]
        [SerializeField] private Vector2 knockbackStrength;
        private float invincibilityTime;

        #endregion

        #region Health Vars

        [Header("Health")] 
        [SerializeField] private Image[] hearts;
        [SerializeField] private Sprite fullHearts;
        [SerializeField] private Sprite emptyHearts;

        #endregion

        #region Data

        [Header("Data")] [SerializeField] private PlayerCombatData playerData;
        private PlayerMovement player;

        #endregion

        #region Components

        private Rigidbody2D rb;

        #endregion

        #endregion

        #region UnityMethods

        private void Awake()
        {
            player = GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody2D>();

            HealthSpriteFinder();
        }

        private void Update()
        {
            invincibilityTime -= Time.deltaTime;
            HealthSpriteUpdater();
        }

        #endregion

        #region healthMethods

        private void HealthSpriteUpdater()
        {
            for (var i = 0; i < hearts.Length; i++)
            {
                playerData.currentHealth = playerData.currentHealth > playerData.maxHealth
                    ? playerData.maxHealth
                    : playerData.currentHealth;

                hearts[i].sprite = i < playerData.currentHealth ? fullHearts : emptyHearts;

                hearts[i].enabled = i < playerData.maxHealth;
            }
        }

        private void HealthSpriteFinder()
        {
            for (var i = 0; i < hearts.Length; i++)
            {
                hearts[i] = GameObject.Find($"Heart ({i})").GetComponent<Image>();
            }
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (invincibilityTime <= 0 || !collision.gameObject.CompareTag("Enemy")) return;
            
            invincibilityTime = 0.5f;
            var enemyDamage = collision.gameObject.GetComponentInParent<EnemyCombat>().enemyData.damage;
            TakeDamage(enemyDamage);
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

        private void TakeKnockBack()
        {
            player.IgnoreRun = true;

            var direction = player.IsFacingRight ? Vector2.left : Vector2.right;
            rb.velocity = Vector3.zero;
            rb.AddForce(knockbackStrength.x * direction, ForceMode2D.Impulse);
            rb.AddForce(knockbackStrength.y * Vector2.up, ForceMode2D.Impulse);
            DOVirtual.DelayedCall(0.5f, () => { player.IgnoreRun = false; });
        }
    }
}