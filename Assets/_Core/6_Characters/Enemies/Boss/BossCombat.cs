using _Core._5_Player.ScriptableObjects;
using UnityEngine;

namespace _Core._6_Characters.Enemies.Boss
{
    public class BossCombat : Destructible
    {
        [SerializeField] private PlayerReferenceData playerData;

        public GameObject GetPlayer()
        {
            return playerData.player;
        }
    }
}