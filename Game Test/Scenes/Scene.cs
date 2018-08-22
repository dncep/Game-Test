using System.Diagnostics;
using System.Drawing;
using System.Linq;

using Game_Test.Components;
using Game_Test.ComponentSystems;
using Game_Test.GameEvents;
using Game_Test.Visuals;
using SdlDotNet.Graphics;

namespace Game_Test.Scenes
{
    class Scene
    {
        public readonly EntityMap Entities;
        public readonly SystemMap Systems;
        public readonly SpriteSet Sprites;
        public readonly EventManager Events;

        public Viewport CurrentViewport { get; set; }

        public float DeltaTime { get; private set; } = 0;

        private readonly Stopwatch timer;

        public Scene()
        {
            Entities = new EntityMap();
            Systems = new SystemMap(this);
            Sprites = new SpriteSet();
            Events = new EventManager();

            Entities.Changed += new EntityChangedEventHandler(NotifyEntityChange);
            CurrentViewport = new Viewport();

            timer = new Stopwatch();

            timer.Start();
        }

        public void Draw(ScreenRenderer r)
        {
            Systems.InvokeRender(r);
        }

        protected virtual void Tick()
        {

        }

        public void InvokeTick()
        {
            DeltaTime = (float) (timer.Elapsed.TotalMilliseconds / 1000d);
            timer.Restart();
            Entities.Flush();

            Systems.InvokeTick();

            Tick();
        }

        private void NotifyEntityChange(EntityChangedEventArgs e)
        {
            foreach (Component component in e.Entity.Components)
            {
                foreach(string eventType in component.ListenedEvents)
                {
                    Events.RegisterEventType(eventType);
                    if (e.WasRemoved) Events.EventDictionary[eventType] -= component.EventFired;
                    else Events.EventDictionary[eventType] += component.EventFired;
                }

                foreach (ComponentSystem system in Systems)
                {
                    if (system.Watching.Contains(component.ComponentName))
                    {
                        if (e.WasRemoved) system.RemoveWatchedComponent(component);
                        else system.AddWatchedComponent(component);
                    }
                }
            }
        }
    }
}
