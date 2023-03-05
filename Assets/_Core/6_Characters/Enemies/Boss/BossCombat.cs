using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss
{
    public class BossCombat : Destructible
    {
        public GameObject player { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}