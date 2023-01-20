using SOEventSystem.Events;
using Spawners.PlayerSpawner.ScriptableObject;
using UnityEngine;

namespace Spawners.PlayerSpawner
{
    //Scene Switcher is the script set on doors
    public class LevelTransition : MonoBehaviour
    {
        [SerializeField]
        private StringEvent onSceneChange;

        [SerializeField] private string nextLevel;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player")) onSceneChange.Invoke(nextLevel);
        }
    }
}
