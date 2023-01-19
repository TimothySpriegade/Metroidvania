using Spawners.PlayerSpawner.Constructs;
using UnityEngine;

namespace Spawners.PlayerSpawner
{
    //Player Spawn holding its own position open for the spawner as well as the information which SpawnPosition (Door) the player had to go through last
    public class PlayerSpawn : MonoBehaviour
    {
        public Vector2 SpawnPosition { get; private set; }
        public SpawnPosition previousDoor;

        private void Awake()
        {
            SpawnPosition = transform.position;
        }
    }
}
