using Spawners.PlayerSpawner.Constructs;
using UnityEngine;

namespace Spawners.PlayerSpawner.SceneSwitcher
{
    //Scene Switcher is the script set on doors
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] private SpawnPositionData data;
        [SerializeField] private SpawnPosition spawnEnum;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player")) data.spawnPosition = spawnEnum;
        }
    }
}
