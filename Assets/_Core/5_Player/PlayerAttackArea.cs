using _Core._5_Player.ScriptableObjects;
using _Core._6_Characters.Enemies;
using _Framework.SOEventSystem;
using _Framework.SOEventSystem.Events;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerAttackArea : MonoBehaviour
    {
        [SerializeField] private PlayerCombatData data;
        [SerializeField] private CameraShakeEvent cameraShake;

        private const string Hittable = "Hittable";

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag(Hittable))
            {
                var hittable = col.GetComponent<Hittable>();
                
                hittable?.OnAttackHit(data.damage, gameObject);
                cameraShake.Invoke(new CameraShakeConfiguration(1, 0.5f, 0.3f));
            }
        }
    }
}