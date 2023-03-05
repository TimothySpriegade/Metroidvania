using _Framework.SOEventSystem;
using SOEventSystem.Events;
using SOEventSystem.UnityEvents;

namespace SOEventSystem.Listeners
{
    public class CameraShakeEventListener : GameEventListener<CameraShakeConfiguration, CameraShakeEvent, UnityCameraShakeEvent>
    {
    }
}
    