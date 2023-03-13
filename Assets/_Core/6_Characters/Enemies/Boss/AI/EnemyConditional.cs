using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.AI
{
    public class EnemyConditional : Conditional
    {
        protected Rigidbody2D rb;
        protected BossCombat bossCombat;
        protected BossEnemy bossEnemy;

        public override void OnAwake()
        {
            rb = GetComponent<Rigidbody2D>();
            bossCombat = GetComponent<BossCombat>();
            bossEnemy = GetComponent<BossEnemy>();
        }
    }
}
