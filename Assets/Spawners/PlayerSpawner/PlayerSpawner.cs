using System.Collections.Generic;
using System.Linq;
using Spawners.PlayerSpawner.Constructs;
using UnityEngine;

namespace Spawners.PlayerSpawner
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private SpawnPositionData spawnPositionData;
        private Transform[] childTransforms;
        private void OnEnable()
        {
            childTransforms = GetAllChildTransforms();

            var spawnIndex = DecideSpawnPoint();
            Instantiate(playerPrefab, childTransforms[spawnIndex].position, Quaternion.identity);
        }

        private Transform[] GetAllChildTransforms()
        {
            var allTransforms = new HashSet<Transform>(GetComponentsInChildren<Transform>());
            allTransforms.Remove(transform);
            return allTransforms.ToArray();
        }

        private int DecideSpawnPoint()
        {
            switch (spawnPositionData.spawnPosition)
            {
                case SpawnPosition.Position1:
                    return 0;
                case SpawnPosition.Position2:
                    return 1;
            }
            return 0;
        }
    }
}
