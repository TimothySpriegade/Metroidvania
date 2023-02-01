using _Core._2_Managers.GameManager.PlayerSpawner.ScriptableObjects;
using UnityEngine;

namespace _Core._2_Managers.GameManager.PlayerSpawner
{
    public class PlayerSpawn : MonoBehaviour
    {
        //If this LevelData matches lastLevel then this Spawn-point is chosen
        public LevelData fromLevel;
        
        //spawn Animation
        public TransitionDirection direction;
    }
}