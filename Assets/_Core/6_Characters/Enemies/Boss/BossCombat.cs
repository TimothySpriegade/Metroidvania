using _Core._5_Player.ScriptableObjects;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss
{
    [RequireComponent(typeof(BossEnemy))]
    public class BossCombat : Destructible
    {
        [SerializeField] private PlayerReferenceData playerData;
        public BossEnemy BossDto { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            BossDto = GetComponent<BossEnemy>();
        }

        public GameObject GetPlayer()
        {
            return playerData.player;
        }
    }
}