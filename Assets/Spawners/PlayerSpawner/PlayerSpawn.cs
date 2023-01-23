using Spawners.PlayerSpawner.ScriptableObjects;
using UnityEngine;

namespace Spawners.PlayerSpawner
{
    public class PlayerSpawn : MonoBehaviour
    {
        //If this LevelData matches lastLevel then this Spawn-point is chosen
        public LevelData FromLevel;
    }
}