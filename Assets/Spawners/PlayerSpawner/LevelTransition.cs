using Player;
using SOEventSystem.Events;
using Spawners.PlayerSpawner.ScriptableObjects;
using UnityEngine;

namespace Spawners.PlayerSpawner
{
    //Scene Switcher is the script set on doors
    public class LevelTransition : MonoBehaviour
    {
        //Transition data manipulation
        [SerializeField] private LevelTransitionData data;
        [SerializeField] private TransitionDirection direction;
        
        //Scene change
        [SerializeField] private StringEvent onSceneChange;
        [SerializeField] private string nextLevel;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                data.direction = direction;
                data.playerWasFacingRight = col.GetComponent<PlayerMovement>().IsFacingRight;
                onSceneChange.Invoke(nextLevel);
                Destroy(col.gameObject);
            }
        }
    }
}