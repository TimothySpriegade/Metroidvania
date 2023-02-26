using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss.AI
{
    public class EnemyConditional : Conditional
    {
        protected Rigidbody2D rb;
        protected Animator animator;
        protected Destructible destructible;
        protected GameObject player;


        public override void OnAwake()
        {
            rb = GetComponent<Rigidbody2D>();
            destructible = GetComponent<Destructible>();
            animator = gameObject.GetComponentInChildren<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
