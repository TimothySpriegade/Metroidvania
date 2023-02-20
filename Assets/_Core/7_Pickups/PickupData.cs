using UnityEngine;

namespace _Core._7_Pickups
{
    [CreateAssetMenu(menuName = "Data/Pickups/PickupData")]
    public class PickupData : ScriptableObject
    {
        public CollectibleEnum collectible;
    }
    
    public enum CollectibleEnum
    {
        Ability,
        Coin,
        Health,
    }
}