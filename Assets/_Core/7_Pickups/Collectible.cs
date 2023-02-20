using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace _Core._7_Pickups
{
    [RequireComponent(typeof(Collider2D))]
    public class Collectible : MonoBehaviour
    {
        [SerializeField] [RequiredField] 
        private PickupData data;
        private const string Player = "Player";
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(Player))
            {
                var player = col.GetComponent<IPickupActor>();
                player?.OnPickup(data.collectible);
            }
            
            // spawn fx
            Destroy(gameObject);
        }
    }
}