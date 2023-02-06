using UnityEngine;

namespace _Core._6_Enemies
{
    public interface IEnemy
    {
        bool isFacingRight { get; }
        
        void EnemyAI();
        void Idle();
        void CheckDirectionToFace(bool isMovingRight);
        void Flip();
        void CollisionCheck(Transform checkPoint, float radius);
    }
}
