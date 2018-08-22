using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Test.Components;
using Game_Test.Entities;
using Game_Test.GameEvents;
using Game_Test.Scenes;
using Game_Test.Util;
using Game_Test.Visuals;

namespace Game_Test.ComponentSystems
{
    class PhysicsSystem : ComponentSystem
    {
        private Vector2D Gravity = new Vector2D(0, -192);

        public PhysicsSystem()
        {
            SystemName = "physics";
            Watching = new string[] { "rigidbody" };
            TickProcessing = true;
            RenderProcessing = true;
        }

        private readonly List<IPolygon> intersections = new List<IPolygon>();

        public override void Tick()
        {
            //Handle velocity
            foreach(RigidBody rb in WatchedComponents)
            {
                Spatial sp = rb.Owner.Components["spatial"] as Spatial;
                rb.PreviousPosition = sp.Position;
                rb.PreviousVelocity = rb.Velocity;
                if(!rb.Immovable)
                {
                    rb.Velocity += Gravity*Owner.DeltaTime;
                    sp.Position += rb.Velocity * Owner.DeltaTime;
                }
            }

            //Sort by X coordinate

            WatchedComponents.Sort((a, b) =>
            {
                return (int) ((GetChronologicalLeft(a.Owner) - GetChronologicalLeft(b.Owner))*1000);
            });

            List<Entity> sweeper = new List<Entity>();

            //Handle collision
            foreach (RigidBody rb in WatchedComponents)
            {
                Entity entity = rb.Owner;
                Spatial sp = entity.Components["spatial"] as Spatial;
                double x = GetChronologicalLeft(rb.Owner);

                while (sweeper.Count > 0 && !IntersectsX(sweeper[0], x))
                {
                    sweeper.RemoveAt(0);
                }

                foreach(Entity other in sweeper)
                {
                    RigidBody rbo = GetRigidBody(other);
                    Spatial spo = GetSpatial(other);

                    IPolygon intersection = rb.Bounds.Offset(sp.Position).Intersect(rbo.Bounds.Offset(spo.Position));


                    if (intersection != null)
                    {
                        if(intersection.GetVertices().Count() >= 3) this.intersections.Add(intersection);

                        Owner.Events.InvokeEvent(new CollisionEvent(GetSpatial(entity).Position, GetRigidBody(entity).Velocity, rb), other, this);
                        Owner.Events.InvokeEvent(new CollisionEvent(GetSpatial(other).Position, GetRigidBody(other).Velocity, rb), entity, this);

                        Vector2D maxVelocity = rb.Velocity;
                        if (rbo.Velocity.Magnitude > maxVelocity.Magnitude) maxVelocity = rbo.Velocity;
                        //Console.WriteLine($"maxVelocity: {maxVelocity}");
                        //Console.WriteLine($"maxVelocity.Angle: {maxVelocity.Angle}");

                        intersection = intersection.Rotate(-maxVelocity.Angle);
                        //Vector2D contact = intersection.GetXFilteredPoint((a, b) => (int)((a - b) * 1000));
                        //FreeVector2D ray = new FreeVector2D(contact, new Vector2D(intersection.GetBounds().Left, contact.Y));

                        double displacement = Math.Min(intersection.GetBounds().Width,maxVelocity.Magnitude);

                        /*foreach(FreeVector2D side in intersection.GetSides())
                        {
                            Vector2D? oppositeContact = side.GetIntersection(ray);
                            if(oppositeContact.HasValue && !oppositeContact.Value.Equals(contact))
                            {
                                displacement = Math.Abs(contact.X - oppositeContact.Value.X);
                                break;
                            }
                        }*/

                        Spatial toMove = sp;
                        RigidBody toStop = rb;
                        
                        if (rb.Immovable) {
                            toMove = spo;
                            toStop = rbo;
                            if(rbo.Immovable)
                            {
                                toMove = null;
                            }
                        }
                        if(toMove != null)
                        {
                            Vector2D displacementVec = new Vector2D(-displacement, 0).Rotate(maxVelocity.Angle);
                            //Console.WriteLine($"displacementVec: {displacementVec}");
                            toMove.Position += displacementVec;
                            toMove.Position.Round();
                            Vector2D newVelocity = toStop.Velocity.Rotate(-maxVelocity.Angle);
                            newVelocity.X = 0;
                            toStop.Velocity = newVelocity.Rotate(maxVelocity.Angle);
                            toStop.Velocity.Round();
                            //Gravity.Y = 0;
                        }

                        //Console.WriteLine(displacement);

                        //Console.WriteLine(contact);

                        //Console.WriteLine(intersection);
                    }
                }

                sweeper.Add(entity);
            }
        }


        private static string ListToString(List<Entity> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Entity entity in list)
            {
                sb.Append(entity);
                sb.Append(", ");
            }
            return sb.ToString();
        }


        private static Spatial GetSpatial(Entity entity)
        {
            return entity.Components["spatial"] as Spatial;
        }

        private static RigidBody GetRigidBody(Entity entity)
        {
            return entity.Components["rigidbody"] as RigidBody;
        }

        private static double GetChronologicalLeft(Entity entity)
        {
            Spatial sp = GetSpatial(entity);
            RigidBody rb = GetRigidBody(entity);

            return Math.Min(
                rb.Bounds.Offset(sp.Position).GetXFilteredPoint((a, b) => (int)((b - a) * 1000)).X,
                rb.Bounds.Offset(rb.PreviousPosition).GetXFilteredPoint((a, b) => (int)((b - a) * 1000)).X
                );
        }

        private static double GetChronologicalRight(Entity entity)
        {
            Spatial sp = GetSpatial(entity);
            RigidBody rb = GetRigidBody(entity);

            return Math.Max(
                rb.Bounds.Offset(sp.Position).GetXFilteredPoint((a, b) => (int)((a - b) * 1000)).X,
                rb.Bounds.Offset(rb.PreviousPosition).GetXFilteredPoint((a, b) => (int)((a - b) * 1000)).X
                );
        }

        private static IPolygon GetAbsoluteBounds(Entity entity)
        {
            IPolygon rect = (entity.Components["rigidbody"] as RigidBody).Bounds;
            Spatial spatial = entity.Components["spatial"] as Spatial;
            rect.Offset(spatial.Position);
            return rect;
        }

        private static bool IntersectsX(Entity entity, double x)
        {
            return GetChronologicalLeft(entity) <= x && x <= GetChronologicalRight(entity);
        }

        public override void Render(ScreenRenderer r)
        {
            Viewport view = Owner.CurrentViewport;
            foreach(RigidBody rb in WatchedComponents)
            {
                Spatial sp = GetSpatial(rb.Owner);
                
                DrawPolygonToScreen(rb.Bounds.Offset(sp.Position), r, view, Color.Red);

                Vector2D location = GameToScreen(sp.Position, r, view);
                Vector2D velocityEnd = GameToScreen(sp.Position + (rb.Velocity)*Owner.DeltaTime*4, r, view);

                r.Screen.Draw(new SdlDotNet.Graphics.Primitives.Line((short)location.X, (short)location.Y, (short)velocityEnd.X, (short)velocityEnd.Y), Color.Blue);
            }
            foreach(IPolygon polygon in intersections)
            {
                DrawPolygonToScreen(polygon, r, view, Color.Purple);
            }
            intersections.Clear();
        }

        private static void DrawPolygonToScreen(IPolygon polygon, ScreenRenderer r, Viewport view, Color color)
        {
            short[] xPoints = new short[polygon.GetVertices().Count()];
            short[] yPoints = new short[polygon.GetVertices().Count()];

            int i = 0;
            foreach (Vector2D vertex in polygon.GetVertices())
            {
                Vector2D converted = GameToScreen(vertex, r, view);

                xPoints[i] = (short)converted.X;
                yPoints[i] = (short)converted.Y;

                i++;
            }
            
            r.Screen.Draw(new SdlDotNet.Graphics.Primitives.Polygon(xPoints, yPoints), color);
        }

        private static Vector2D GameToScreen(Vector2D point, ScreenRenderer r, Viewport view)
        {
            double x = point.X;
            double y = -point.Y;

            x -= view.X;
            y += view.Y;

            x *= r.ScreenSize.Width / r.ScreenResolution.Width;
            y *= r.ScreenSize.Height / r.ScreenResolution.Height;

            x *= view.Scale;
            y *= view.Scale;

            x += r.ScreenSize.Width / 2;
            y += r.ScreenSize.Height / 2;

            return new Vector2D(x, y);
        }
    }
}
