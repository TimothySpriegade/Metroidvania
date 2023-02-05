using System;
using System.Collections.Generic;
using System.Linq;
using _Core._2_Managers.GameManager.PlayerSpawner.ScriptableObjects;
using _Core._5_Player;
using UnityEngine;

namespace _Core._2_Managers.GameManager.PlayerSpawner
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private LevelTransitionData data;
        [SerializeField] private LevelData currentLevel;
        private Vector2 spawnPosition;
        private TransitionDirection direction;

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
            foreach (var child in allChildren.Where(child => child.fromLevel == data.lastLevel))
            {
                return ExtractChildInformation(child);
            }

            //Returns first playerSpawn found
            if (allChildren.Count > 0) return ExtractChildInformation(allChildren.First());


            //Returns the middle if there is no playerSpawn
            direction = TransitionDirection.Down;
            return Vector2.zero;
        }

        private void TryInstantiatePlayer()
        {
            try
            {
                var player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

                if (ShouldTurnPlayer())
                {
                    TurnPlayer(player);
                }

                player.GetComponent<PlayerAnimator>()
                    .EnteringSceneAnimation(direction, data.playerWasFacingRight);
            }
            catch (Exception)
            {
                Debug.LogWarning("Player prefab is missing from PlayerSpawner");
            }
        }

        private bool ShouldTurnPlayer()
        {
            //Dont turn if the player goes left and only go right if the player was facing right or enters a RightAnimation
            return direction != TransitionDirection.Left && data.playerWasFacingRight
                   || direction == TransitionDirection.Right;
        }

        private static void TurnPlayer(GameObject player)
        {
            var scale = player.transform.localScale;
            scale.x *= -1;
            player.transform.localScale = scale;
            player.GetComponent<PlayerMovement>().IsFacingRight = true;
        }

        private Vector2 ExtractChildInformation(PlayerSpawn child)
        {
            direction = child.direction;
            return child.transform.position;
        }
    }
}