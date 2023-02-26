using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.AI
{
    public class EnemyAction : Action
    {
        protected Rigidbody2D rb;
        protected Animator animator;
        protected Destructible destructible;
        // TODO add playerUtils

        public override void OnAwake()
        {
            rb = GetComponent<Rigidbody2D>();
            destructible = GetComponent<Destructible>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }
    }
}
