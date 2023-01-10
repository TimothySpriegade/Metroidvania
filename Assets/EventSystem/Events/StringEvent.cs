using UnityEngine;

namespace EventSystem.Events
{
    [CreateAssetMenu(menuName = "GameEvents/Game Event <string>")]
    public class StringEvent : GameEvent<string>
    {
    }
}
