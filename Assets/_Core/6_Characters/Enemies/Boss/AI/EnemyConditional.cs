using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.AI
{
    public class EnemyConditional : Conditional
    {
        private Rigidbody2D rb;
        private Animator animator;
        private Destructible destructible;
        // TODO add playerUtils


        public override void OnAwake()
        {
            rb = GetComponent<Rigidbody2D>();
            destructible = GetComponent<Destructible>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }
    }
}
