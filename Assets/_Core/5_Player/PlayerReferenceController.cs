using _Core._5_Player.ScriptableObjects;
using UnityEngine;

namespace _Core._5_Player
{
    public class PlayerReferenceController : MonoBehaviour
    {
        [SerializeField] private PlayerReferenceData referenceData;
        
        private void Awake()
        {
            referenceData.player = gameObject;
        }

        private void OnDisable()
        {
            referenceData.player = null;
        }
    }
}