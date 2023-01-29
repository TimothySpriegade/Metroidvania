using _Core._2_Managers.GameManager.PlayerSpawner.ScriptableObjects;
using _Core._5_Player;
using SOEventSystem.Events;
using UnityEngine;

namespace _Core._2_Managers.GameManager.PlayerSpawner
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