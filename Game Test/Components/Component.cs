using Game_Test.Entities;
using Game_Test.GameEvents;

namespace Game_Test.Components
{
    abstract class Component
    {
        public Entity Owner { get; set; }
        public string ComponentName { get; protected set; }
        public string[] ListenedEvents { get; protected set; } = new string[0];

        public virtual void EventFired(object sender, Event e)
        {
        }
    }
}
