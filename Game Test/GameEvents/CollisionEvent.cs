using Game_Test.Components;
using Game_Test.Util;

namespace Game_Test.GameEvents
{
    class CollisionEvent : Event
    {
        public Vector2D Position { get; private set; }
        public Vector2D Velocity { get; private set; }

        public CollisionEvent(Vector2D position, Vector2D velocity, Component sender) : base("collision", sender)
        {
            this.Position = position;
            this.Velocity = velocity;
        }

        public override string ToString() => $"Collision by: {Sender.Owner}";
    }
}
