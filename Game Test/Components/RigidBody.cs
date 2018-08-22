using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.GameEvents;
using Game_Test.Util;

namespace Game_Test.Components
{
    class RigidBody : Component
    {
        public IPolygon Bounds { get; set; }
        public float Mass { get; set; }
        public bool Immovable { get; set; }
        public Vector2D PreviousPosition { get; internal set; }
        public Vector2D PreviousVelocity { get; internal set; }
        public List<Vector2D> Forces { get; internal set; }

        public Vector2D Velocity = new Vector2D();

        public RigidBody(IPolygon bounds, float mass, bool immovable)
        {
            ComponentName = "rigidbody";
            ListenedEvents = new string[] { "collision" };
            this.Bounds = bounds;
            this.Mass = mass;
            this.Immovable = immovable;
        }

        //private float restitutionCoefficient = 0.6f;

        public override void EventFired(object sender, Event e)
        {
            /*if(e is CollisionEvent && !Immovable)
            {
                Console.WriteLine(e);
                this.Velocity = -restitutionCoefficient * this.Velocity;
            }*/
        }
    }
}
