using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss
{
    public class BoneSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject boneSpawnerParent;
        private Collider2D[] boneSpawners;
        
        public Collider2D[] GetBoneSpawners()
        {
            return boneSpawners ??= boneSpawnerParent.GetComponents<Collider2D>();
        }
    }
}
