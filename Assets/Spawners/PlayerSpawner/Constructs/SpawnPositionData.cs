using UnityEngine;

namespace Spawners.PlayerSpawner.Constructs
{
    [CreateAssetMenu(menuName = "Data/Spawner/Spawn Position Data")]
    public class SpawnPositionData : ScriptableObject
    {
        public SpawnPosition spawnPosition;
    }
}
