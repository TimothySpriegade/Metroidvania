using _Core._5_Player.ScriptableObjects;
using _Core._7_Pickups;
using _Framework;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerPickup : MonoBehaviour, IPickupActor
    {
        [SerializeField] private PlayerUnlockableData data;
        public void OnPickup(CollectibleEnum collectible)
        {
            switch (collectible)
            {
                case CollectibleEnum.Ability:
                    this.Log("Collected ability");
                    data.unlockedDash = true;
                    break;
                default:
                    this.LogWarning("Unimplemented collectible type");
                    break;
            }
        }
    }
}