using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Player;
using Spawners.PlayerSpawner.ScriptableObjects;
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
            //Gets spawn position by comparing child FromLevels with LevelTransitionData's LastLevel
            spawnPosition = GetSpawnPosition();
            
            //Creates Player at Spawn-point
            TryInstantiatePlayer();

            //Sets LastLevel to CurrentLevel (LastLevel is only relevant at the start of the scene so it can already be replaced after spawning the player)
            data.lastLevel = currentLevel;
        }

        private Vector2 GetSpawnPosition()
        {
            //Get all Children
            var allChildren = new List<PlayerSpawn>(GetComponentsInChildren<PlayerSpawn>());

            //Search for the child who's FromLevel matches the last Level
            foreach (var child in allChildren.Where(child => child.FromLevel == data.lastLevel))
            {
                return child.transform.position;
            }

            //Returns first playerSpawn found
            if (allChildren.Count > 0) return allChildren.First().transform.position;
            
            
            //Returns the middle if there is no playerSpawn
            return Vector2.zero;
        }

        private void TryInstantiatePlayer()
        {
            try
            {
                var player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

                if (data.playerWasFacingRight)
                {
                    var scale = player.transform.localScale;
                    scale.x *= -1;
                    player.transform.localScale = scale;
                    player.GetComponent<PlayerMovement>().IsFacingRight = true;
                }
                
                if (data.direction != TransitionDirection.Down)
                {
                    player.GetComponent<PlayerAnimator>().EnteringSceneAnimation(data.direction, data.playerWasFacingRight);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
