using UnityEngine;

namespace SOEventSystem.Events
{
    [CreateAssetMenu(menuName = "GameEvents/Game Event <Void>")]
    public class VoidEvent : GameEvent<Void>
    {
        public void Invoke() => Invoke(new Void());
    }
}
