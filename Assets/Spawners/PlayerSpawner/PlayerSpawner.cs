using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Spawners.PlayerSpawner.ScriptableObject;
using UnityEngine;

namespace Spawners.PlayerSpawner
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private LevelTransitionData data;
        [SerializeField] private LevelData currentLevel;
        private Vector2 spawnPosition;
        
        private void OnEnable()
        {
            //Gets spawn position by comparing child FromLevels with LevelData's LastLevel
            spawnPosition = GetSpawnPosition();

            //TODO what if there was no PlayerSpawner before? Should we place the currentLevel/LastLevel logic into a SO or do we have some null handling?
            
            //Creates Player at Spawn-point
            TryInstantiatePlayer();

            //Sets LastLevel to CurrentLevel (LastLevel is only relevant at the start of the scene so it can already be replaced after spawning the player)
            data.lastLevel = currentLevel;
        }

        private Vector2 GetSpawnPosition()
        {
            //Get all Children
            var allChildren = new HashSet<PlayerSpawn>(GetComponentsInChildren<PlayerSpawn>());

            //Search for the child who's FromLevel matches the last Level
            foreach (var child in allChildren.Where(child => child.FromLevel == data.lastLevel))
            {
                return child.transform.position;
            }
            
            //Returns the middle if no child with wanted parameters exists
            return Vector2.zero;
        }

        private void TryInstantiatePlayer()
        {
            try
            {
                var player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

                if (data.direction != TransitionDirection.Down)
                {
                    //TODO Coroutine
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        
        private IEnumerator UpAnimation(bool toRight)
        {
            yield return null;
        }
        
        private IEnumerator SideAnimation(bool toRight)
        {
            yield return null;
        }

    }
}
