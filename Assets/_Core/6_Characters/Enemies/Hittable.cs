using System.Collections;
using UnityEngine;

namespace _Core._6_Characters.Enemies
{
    public class Hittable : MonoBehaviour
    {
        [Header("Hittable")]
        [SerializeField] protected bool disableHitEffect = false;

        public Transform spriteParent;
        public Material hitMaterial;

        private SpriteRenderer sprite;
        private Material defaultMaterial;

        protected virtual void Awake()
        {
            // Find all child sprite renderers
            var spriteParentTransform = spriteParent == null ? transform : spriteParent;
            sprite = spriteParentTransform.GetComponentInChildren<SpriteRenderer>();

            defaultMaterial = sprite.material;
        }

        public virtual void OnAttackHit(int damage)
        {
            if (disableHitEffect) return;

            // Impact color flash
            sprite.material = hitMaterial;
            StartCoroutine(ResetMaterial(0.1f));
        }

        private IEnumerator ResetMaterial(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            sprite.material = defaultMaterial;
        }
    }
}