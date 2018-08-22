
using Game_Test.Components;

namespace Game_Test.GameEvents
{
    class Event
    {
        public string EventType { get; private set; }
        public Component Sender { get; }

        public Event(string eventType, Component sender)
        {
            EventType = eventType;
            Sender = sender;
        }
    }
}
