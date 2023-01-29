using SOEventSystem.Events;
using SOEventSystem.UnityEvents;

namespace SOEventSystem.Listeners
{
    public class VoidEventListener : GameEventListener<Void, VoidEvent, UnityVoidEvent>
    {
    }
}