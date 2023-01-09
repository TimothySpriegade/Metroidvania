using UnityEngine;

namespace EventSystem.Events
{
    [CreateAssetMenu(menuName = "GameEvents/Game Event <Void>")]
    public class VoidEvent : GameEvent<Void>
    {
        public void Raise() => Raise(new Void());
    }
}
